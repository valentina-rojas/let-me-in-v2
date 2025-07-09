using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{


    private float typingTime = 0.05f;
    private bool isMouseOver = false;
    private bool didDialogueStart;
    private int lineIndex;
    private bool hasInteracted = false;
    private bool esDialogoRespuesta = false;


    private string[] dialogueLines;
    private CharacterAttributes characterAttributes;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    [SerializeField] private GameObject dialoguePanelPersonaje;
    [SerializeField] private GameObject dialoguePanelGuardia;
    [SerializeField] private TMP_Text dialogueTextPersonaje;
    [SerializeField] private TMP_Text dialogueTextGuardia;
    [SerializeField] private Button botonSiguientePersonaje;
    [SerializeField] private Button botonSiguienteGuardia;




    private void Start()

    {


        UIManager uiManager = FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            dialoguePanelPersonaje = uiManager.GetDialoguePanelPersonaje();
            dialoguePanelGuardia = uiManager.GetDialoguePanelGuardia();
            dialogueTextPersonaje = uiManager.GetDialogueTextPersonaje();
            dialogueTextGuardia = uiManager.GetDialogueTextGuardia();
            botonSiguientePersonaje = uiManager.GetBotonSiguientePersonaje();
            botonSiguienteGuardia = uiManager.GetBotonSiguienteGuardia();

        }
        else
        {
            Debug.LogError("UIManager no encontrado en la escena.");
        }


        characterAttributes = GetComponent<CharacterAttributes>();

        botonSiguienteGuardia.onClick.AddListener(NextDialogueLine);
        botonSiguientePersonaje.onClick.AddListener(NextDialogueLine);

    }

    private void StartDialogue()
    {
        esDialogoRespuesta = false;
        if (characterAttributes == null) return;

        int cantidadGuardia = characterAttributes.dialogosGuardia.Count;
        int cantidadPersonaje = characterAttributes.dialogosPersonaje.Count;
        int longitudFinal = Mathf.Min(cantidadGuardia, cantidadPersonaje) * 2;

        dialogueLines = new string[longitudFinal];

        for (int i = 0; i < longitudFinal / 2; i++)
        {
            dialogueLines[i * 2] = characterAttributes.dialogosGuardia[i];       // Guardia
            dialogueLines[i * 2 + 1] = characterAttributes.dialogosPersonaje[i]; // Personaje
        }

        if (dialogueLines.Length == 0) return;

        didDialogueStart = true;
        lineIndex = 0;

        dialoguePanelGuardia.SetActive(false);
        dialoguePanelPersonaje.SetActive(false);
        botonSiguienteGuardia.gameObject.SetActive(false);
        botonSiguientePersonaje.gameObject.SetActive(false);

        typingCoroutine = StartCoroutine(ShowLine());
    }

    public void ComenzarDialogoRespuesta(string[] lineasRespuesta)
    {
        dialogueLines = lineasRespuesta;
        lineIndex = 0;
        didDialogueStart = true;
        esDialogoRespuesta = true;

        dialoguePanelGuardia.SetActive(false);
        dialoguePanelPersonaje.SetActive(true); // Asumiendo que es el personaje quien habla

        typingCoroutine = StartCoroutine(ShowLine());
    }


    private void Update()
    {
        if (didDialogueStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextDialogueLine();
            }
            else if (Input.GetKeyDown(KeyCode.Return)) // Enter
            {
                OmitirDialogo();
            }
        }
    }

    public void NextDialogueLine()
    {
        if (!didDialogueStart || dialogueLines == null) return;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);

            if (esDialogoRespuesta)
            {
                dialogueTextPersonaje.text = dialogueLines[lineIndex];
                botonSiguientePersonaje.gameObject.SetActive(true);
            }
            else if (lineIndex % 2 == 0)
            {
                dialogueTextGuardia.text = dialogueLines[lineIndex];
                botonSiguienteGuardia.gameObject.SetActive(true);
            }
            else
            {
                dialogueTextPersonaje.text = dialogueLines[lineIndex];
                botonSiguientePersonaje.gameObject.SetActive(true);
            }

            isTyping = false;
            return;
        }

        lineIndex++;

        if (lineIndex < dialogueLines.Length)
        {
            typingCoroutine = StartCoroutine(ShowLine());
        }
        else
        {
            FinalizarDialogo();
        }
    }



    private IEnumerator ShowLine()
    {
        isTyping = true;

        // Alternar entre guardia (par) y personaje (impar)
        bool hablaGuardia = lineIndex % 2 == 0;

        // Mostrar texto en el panel correspondiente
        if (hablaGuardia)
        {
            dialoguePanelGuardia.SetActive(true);
            dialoguePanelPersonaje.SetActive(false);
            dialogueTextGuardia.text = "";
            botonSiguienteGuardia.gameObject.SetActive(false);
            foreach (char ch in dialogueLines[lineIndex])
            {
                dialogueTextGuardia.text += ch;
                yield return new WaitForSeconds(typingTime);
            }
            botonSiguienteGuardia.gameObject.SetActive(true);
        }
        else
        {
            dialoguePanelPersonaje.SetActive(true);
            dialoguePanelGuardia.SetActive(false);
            dialogueTextPersonaje.text = "";
            botonSiguientePersonaje.gameObject.SetActive(false);
            foreach (char ch in dialogueLines[lineIndex])
            {
                dialogueTextPersonaje.text += ch;
                yield return new WaitForSeconds(typingTime);
            }
            botonSiguientePersonaje.gameObject.SetActive(true);
        }

        isTyping = false;
    }


    private void FinalizarDialogo()
    {
        didDialogueStart = false;
        hasInteracted = true;

        CharacterManager characterManager = FindObjectsByType<CharacterManager>(FindObjectsSortMode.None)[0];
        if (characterManager != null && characterAttributes != null)
        {
            characterManager.AtenderPersonaje(characterAttributes);
        }

        if (LeverController.instance != null)
        {
            Debug.Log("✅ Llamando a ActivarPalanca");
            LeverController.instance.ActivarPalanca();
        }
        else
        {
            Debug.LogError("❌ LeverController.instance es null al finalizar diálogo");
        }

        dialoguePanelPersonaje.SetActive(false);
        dialoguePanelGuardia.SetActive(false);
    }

    public void EnableDialogue()
    {
        if (!hasInteracted)
        {
            isMouseOver = true;
            StartDialogue();
        }
    }

    public void OmitirDialogo()
    {
        if (!didDialogueStart || dialogueLines == null) return;

        // Mostrar directamente la última línea del diálogo
        StopAllCoroutines();
        isTyping = false;
        lineIndex = dialogueLines.Length;

        FinalizarDialogo();
    }


    public bool HaTerminadoElDialogo()
    {
        return !didDialogueStart;
    }


}
