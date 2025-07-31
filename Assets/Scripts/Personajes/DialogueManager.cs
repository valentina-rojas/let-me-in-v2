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
    public bool medicoUsado = false;

    private string[] dialogueLines;
    private string currentFullLine = ""; // NUEVO
    private CharacterAttributes characterAttributes;
    public AggressiveNPCs aggressiveNPCs;

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

        // Validar: el personaje debe tener exactamente una línea más
        if (cantidadPersonaje != cantidadGuardia + 1)
        {
            Debug.LogError($"Cantidad incorrecta de líneas: Personaje = {cantidadPersonaje}, Guardia = {cantidadGuardia}. El personaje debe tener exactamente UNA más.");
            return;
        }

        int totalLineas = cantidadPersonaje + cantidadGuardia; // Ej: 3 + 2 = 5
        dialogueLines = new string[totalLineas];

        int index = 0;
        for (int i = 0; i < cantidadGuardia; i++)
        {
            dialogueLines[index++] = characterAttributes.dialogosPersonaje[i]; // personaje
            dialogueLines[index++] = characterAttributes.dialogosGuardia[i];   // guardia
        }

        // Última línea del personaje (extra)
        dialogueLines[index] = characterAttributes.dialogosPersonaje[cantidadPersonaje - 1];

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
        dialoguePanelPersonaje.SetActive(true); // El personaje habla
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
            else if (Input.GetKeyDown(KeyCode.Return))
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
           // AudioManager.instance.vozGuardia.Stop();
            //AudioManager.instance.vozPersonaje.Stop();


            StopCoroutine(typingCoroutine);
            isTyping = false;

            if (esDialogoRespuesta)
            {
                dialogueTextPersonaje.text = currentFullLine;
                botonSiguientePersonaje.gameObject.SetActive(true);
            }
            else
            {
                bool hablaGuardia = lineIndex % 2 != 0; // IMPAR = guardia

                if (hablaGuardia)
                {
                    dialogueTextGuardia.text = currentFullLine;
                    botonSiguienteGuardia.gameObject.SetActive(true);
                }
                else
                {
                    dialogueTextPersonaje.text = currentFullLine;
                    botonSiguientePersonaje.gameObject.SetActive(true);
                }
            }

            return; // No avanzar aún
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
        currentFullLine = dialogueLines[lineIndex]; // NUEVO

        if (esDialogoRespuesta)
        {
            dialoguePanelGuardia.SetActive(false);
            dialoguePanelPersonaje.SetActive(true);
            dialogueTextPersonaje.text = "";
            botonSiguientePersonaje.gameObject.SetActive(false);

           // AudioManager.instance.vozPersonaje.Play();

            foreach (char ch in currentFullLine)
            {
                dialogueTextPersonaje.text += ch;
                yield return new WaitForSeconds(typingTime);
            }


            botonSiguientePersonaje.gameObject.SetActive(true);


        }
        else
        {
            bool hablaGuardia = lineIndex % 2 != 0;

            if (hablaGuardia)
            {
                dialoguePanelGuardia.SetActive(true);
                dialoguePanelPersonaje.SetActive(false);
                dialogueTextGuardia.text = "";
                botonSiguienteGuardia.gameObject.SetActive(false);

               // AudioManager.instance.vozGuardia.Play();

                foreach (char ch in currentFullLine)
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

               //  AudioManager.instance.vozPersonaje.Play();

                foreach (char ch in currentFullLine)
                {
                    dialogueTextPersonaje.text += ch;
                    yield return new WaitForSeconds(typingTime);
                }


                botonSiguientePersonaje.gameObject.SetActive(true);


            }
        }

        isTyping = false;
    }

    private void FinalizarDialogo()
    {
      //  AudioManager.instance.vozGuardia.Stop();
       //AudioManager.instance.vozPersonaje.Stop();

        didDialogueStart = false;
        hasInteracted = true;

        CharacterManager characterManager = FindObjectsByType<CharacterManager>(FindObjectsSortMode.None)[0];
        if (characterManager != null && characterAttributes != null)
        {
            characterManager.AtenderPersonaje(characterAttributes);
        }

        if (!esDialogoRespuesta)
        {
            if (characterAttributes.esAgresivo)
            {
                AggressiveNPCs.instance.MostrarComportamientoAgresivo();
            }
            else
            {
                if (LeverController.instance != null)
                {
                    LeverController.instance.ActivarPalanca();

                    if (!medicoUsado)
                    {
                        CheckCondition checkCondition = FindFirstObjectByType<CheckCondition>();
                        if (checkCondition != null)
                        {
                            checkCondition.botonMedico.interactable = true;
                        }
                    }
                }
                else
                {
                    Debug.LogError("❌ LeverController.instance es null al finalizar diálogo");
                }
            }
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
