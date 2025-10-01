using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    public TextAsset inkJSON; // Story Ink
    public Story currentStory;

    [Header("UI")]
    public GameObject dialoguePanel; // Panel principal
    public TMP_Text dialogueText;    // Texto del diálogo

    [Header("Typing")]
    public float typingTime = 0.02f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullLine = "";

    [Header("Estado")]
    public bool medicoUsado = false;  // Para CheckCondition.cs
    private bool didDialogueStart = false;

  
    [Header("Opciones (Canvas)")]
public List<Button> optionButtons; // Arrastrás los botones desde el Canvas


    [Header("Botón Continuar")]
    public Button botonSiguiente;

    #region Singleton
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Hay más de un DialogueManager en la escena");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }
    #endregion

    private void Start()
    {
        dialoguePanel.SetActive(false);

        if (botonSiguiente != null)
        {
            botonSiguiente.onClick.AddListener(NextDialogueLine);
            botonSiguiente.gameObject.SetActive(false);
        }
    }

    public void InicializarHistoria()
    {
        if (inkJSON != null)
            currentStory = new Story(inkJSON.text);
        else
            Debug.LogError("No se asignó Ink JSON al DialogueManager.");
    }

    public void EnableDialogue()
    {
        if (currentStory == null) InicializarHistoria();
        didDialogueStart = true;
        ShowNextLine();
    }

    public void ComenzarDialogoRespuesta(string nombreNodo)
    {
        if (currentStory == null) InicializarHistoria();
        currentStory.ChoosePathString(nombreNodo);
        didDialogueStart = true;
        ShowNextLine();
    }

    private void ShowNextLine()
    {
        ClearOptions();

        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(false);

        // Si ya no queda texto, mostrar opciones o terminar diálogo
        if (!currentStory.canContinue)
        {
            ShowChoices();

            if (currentStory.currentChoices.Count == 0)
                FinalizarDialogo();

            return;
        }

        // Mostrar la próxima línea
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

        if (string.IsNullOrEmpty(line))
        {
            dialogueText.text = "";
            isTyping = false;
            if (botonSiguiente != null)
                botonSiguiente.gameObject.SetActive(true);
            yield break;
        }

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

    public bool HaTerminadoElDialogo()
    {
        return !didDialogueStart;
    }

    private void FinalizarDialogo()
    {
        didDialogueStart = false;
        dialoguePanel.SetActive(false);
        ClearOptions();
    }

    #region Opciones Ink
 private void ShowChoices()
{
    Debug.Log("Entrando a ShowChoices");
Debug.Log("optionButtons.Count = " + optionButtons.Count);
for (int i = 0; i < optionButtons.Count; i++)
{
    Debug.Log("Botón en lista: " + optionButtons[i].name);
}
Debug.Log("currentStory.currentChoices.Count = " + currentStory.currentChoices.Count);

    Debug.Log("Choices disponibles: " + currentStory.currentChoices.Count);

    dialoguePanel.SetActive(true);

    // Primero desactivo todos los botones
    foreach (var btn in optionButtons)
    {
        btn.gameObject.SetActive(false);
        btn.onClick.RemoveAllListeners();
    }

    // Activar solo los botones necesarios
    for (int i = 0; i < currentStory.currentChoices.Count; i++)
    {
        if (i >= optionButtons.Count)
        {
            Debug.LogWarning("Más opciones que botones disponibles. Necesitas más botones en el Canvas.");
            break; // evita ArgumentOutOfRangeException
        }

        Choice choice = currentStory.currentChoices[i];
        Button btn = optionButtons[i];

        btn.gameObject.SetActive(true);

     TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();
if (txt != null)
{
    txt.text = choice.text;
    Debug.Log($"Botón {btn.name} texto asignado: {txt.text}");
}
else
{
    Debug.LogWarning("No hay TMP_Text en el botón " + btn.name);
}

        Choice capturedChoice = choice;
        btn.onClick.AddListener(() => OnClickChoice(capturedChoice));
    }
}



    private void OnClickChoice(Choice choice)
    {
        currentStory.ChooseChoiceIndex(choice.index);
        ShowNextLine();
    }

  private void ClearOptions()
{
    foreach (var btn in optionButtons)
    {
        btn.gameObject.SetActive(false);
        btn.onClick.RemoveAllListeners();
    }
}

    #endregion
}
