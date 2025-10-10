using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Button botonSiguiente;
    public List<Button> optionButtons;

    [Header("Typing")]
    public float typingTime = 0.02f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullLine = "";

    [Header("Estado")]
    private bool didDialogueStart = false;

    [Header("Estado Médico")]
    public bool medicoUsado = false;

    [Header("Personaje")]
    private CharacterAttributes personajeActual;
    public bool esDialogoRespuesta = false;

    private Story currentStory;

    #region Singleton
    public static DialogueManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Más de un DialogueManager en la escena.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    private void Start()
    {
        if (dialoguePanel == null)
            dialoguePanel = GameManager.instance.uiManager.GetDialoguePanelPersonaje();

        if (dialogueText == null)
            dialogueText = GameManager.instance.uiManager.GetDialogueTextPersonaje();

        if (botonSiguiente == null)
            botonSiguiente = GameManager.instance.uiManager.GetBotonSiguientePersonaje();

        optionButtons = GameManager.instance.uiManager.GetOptionButtons();

        dialoguePanel.SetActive(false);
        if (botonSiguiente != null)
            botonSiguiente.onClick.AddListener(NextDialogueLine);
    }

    private void Update()
    {
        // 🔹 Omitir diálogo completo con ENTER
        if (didDialogueStart && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            Debug.Log("Omitiendo diálogo con Enter...");
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine); // Detener el tipeo si está activo

            FinalizarDialogo();
        }
    }

    public void IniciarDialogoDePersonaje(CharacterAttributes personaje, string nodo, bool esRespuesta)
    {
        Debug.Log("----- IniciarDialogoDePersonaje -----");

        if (personaje == null)
        {
            Debug.LogError("Personaje es null!");
            return;
        }

        if (personaje.inkJSON == null)
        {
            Debug.LogError($"Ink JSON no asignado en el personaje {personaje.nombre}");
            return;
        }

        personajeActual = personaje;
        esDialogoRespuesta = esRespuesta;

        try
        {
            currentStory = new Story(personaje.inkJSON.text);
            currentStory.ChoosePathString(nodo);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al crear o elegir nodo en Story: {e.Message}");
            return;
        }

        didDialogueStart = true;
        ShowNextLine();
    }

    private void ShowNextLine()
    {
        if (currentStory == null)
        {
            Debug.LogError("currentStory es null en ShowNextLine");
            return;
        }

        ClearOptions();

        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(false);

        if (!currentStory.canContinue)
        {
            ShowChoices();

            if (currentStory.currentChoices.Count == 0)
                FinalizarDialogo();

            return;
        }

        currentFullLine = currentStory.Continue().Trim();
        dialoguePanel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(currentFullLine));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingTime);
        }

        isTyping = false;

        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(true);
    }

    public void NextDialogueLine()
    {
        if (isTyping)
        {
            dialogueText.text = currentFullLine;
            isTyping = false;
            if (botonSiguiente != null)
                botonSiguiente.gameObject.SetActive(true);
            return;
        }

        ShowNextLine();
    }

    private void ShowChoices()
    {
        if (currentStory == null) return;

        dialoguePanel.SetActive(true);

        foreach (var btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < currentStory.currentChoices.Count; i++)
        {
            if (i >= optionButtons.Count)
            {
                Debug.LogWarning("Más opciones que botones disponibles.");
                break;
            }

            Choice choice = currentStory.currentChoices[i];
            Button btn = optionButtons[i];
            btn.gameObject.SetActive(true);

            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();
            if (txt != null)
                txt.text = choice.text;

            Choice capturedChoice = choice;
            btn.onClick.AddListener(() =>
            {
                currentStory.ChooseChoiceIndex(capturedChoice.index);
                ShowNextLine();
            });
        }
    }

    private void ClearOptions()
    {
        foreach (var btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }
    }

    private void FinalizarDialogo()
    {
        didDialogueStart = false;
        dialoguePanel.SetActive(false);
        ClearOptions();

        if (!esDialogoRespuesta)
        {
            Debug.Log("Diálogo inicial terminado");

            if (personajeActual != null && personajeActual.esAgresivo)
            {
                Debug.Log($"Personaje {personajeActual.nombre} es agresivo → ejecutando comportamiento.");
                AggressiveNPCs.instance?.MostrarComportamientoAgresivo();
            }
            else
            {
                GameManager.instance.OnDialogoInicialTerminado();

                if (!medicoUsado)
                {
                    CheckCondition checkCondition = FindFirstObjectByType<CheckCondition>();
                    if (checkCondition != null && checkCondition.botonMedico != null)
                    {
                        checkCondition.botonMedico.interactable = true;
                        Debug.Log("Botón médico habilitado (no fue usado aún)");
                    }
                }
            }
        }
        else
        {
            CharacterManager.instance?.AtenderPersonaje(personajeActual);
        }

        esDialogoRespuesta = false;
    }

    public bool HaTerminadoElDialogo()
    {
        return !didDialogueStart;
    }
}
