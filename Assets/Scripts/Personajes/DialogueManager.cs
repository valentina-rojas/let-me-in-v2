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

    public int dialogosOmitidos = 0;

    private string[] dialogueLines;
    private string currentFullLine = ""; // NUEVO
    private CharacterAttributes characterAttributes;
    public AggressiveNPCs aggressiveNPCs;

    private Coroutine typingCoroutine;
    private Coroutine blinkCoroutine;
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
        StopCoroutine(typingCoroutine);
        isTyping = false;

          // Parar las voces al adelantar
        AudioManager.instance.vozGuardia.Stop();
        AudioManager.instance.vozPersonaje.Stop();

        TMP_Text activeText;
        Button activeButton;

        if (esDialogoRespuesta)
        {
            activeText = dialogueTextPersonaje;
            activeButton = botonSiguientePersonaje;
        }
        else
        {
            bool hablaGuardia = lineIndex % 2 != 0; // IMPAR = guardia
            if (hablaGuardia)
            {
                activeText = dialogueTextGuardia;
                activeButton = botonSiguienteGuardia;
            }
            else
            {
                activeText = dialogueTextPersonaje;
                activeButton = botonSiguientePersonaje;
            }
        }

        // Mostrar línea completa
        activeText.text = currentFullLine;
        activeButton.gameObject.SetActive(true);
 
        // 🔹 Iniciar cursor titilante incluso si se adelantó el texto
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkCursor(activeText, currentFullLine));

        GameManager.instance.ReproducirAnimacionPestañar();
  

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
        currentFullLine = dialogueLines[lineIndex];

        TMP_Text activeText;
        Button activeButton;

        // Determinar quién habla y qué UI usar
        if (esDialogoRespuesta)
        {
              // activar animación de hablar
            GameManager.instance.ReproducirAnimacionHablar();

            dialoguePanelGuardia.SetActive(false);
            dialoguePanelPersonaje.SetActive(true);
            dialogueTextPersonaje.text = "";
            botonSiguientePersonaje.gameObject.SetActive(false);

            activeText = dialogueTextPersonaje;
            activeButton = botonSiguientePersonaje;

               // Reproducir voz personaje
            AudioManager.instance.vozGuardia.Stop();
            AudioManager.instance.vozPersonaje.Play();
        }
        else
        {
            
            bool hablaGuardia = lineIndex % 2 != 0;

            if (hablaGuardia)
            {
                 // está hablando el guardia, entonces animación personaje debe ser pestañar
                GameManager.instance.ReproducirAnimacionPestañar();

                dialoguePanelGuardia.SetActive(true);
                dialoguePanelPersonaje.SetActive(false);
                dialogueTextGuardia.text = "";
                botonSiguienteGuardia.gameObject.SetActive(false);

                activeText = dialogueTextGuardia;
                activeButton = botonSiguienteGuardia;

                  // Reproducir voz guardia
                AudioManager.instance.vozPersonaje.Stop();
                AudioManager.instance.vozGuardia.Play();
            }
            else
            {
                   // Activar triggerTalk al empezar a hablar
                 GameManager.instance.ReproducirAnimacionHablar();
      
                dialoguePanelPersonaje.SetActive(true);
                dialoguePanelGuardia.SetActive(false);
                dialogueTextPersonaje.text = "";
                botonSiguientePersonaje.gameObject.SetActive(false);

                activeText = dialogueTextPersonaje;
                activeButton = botonSiguientePersonaje;

                // Reproducir voz personaje
                AudioManager.instance.vozGuardia.Stop();
                AudioManager.instance.vozPersonaje.Play();
            }
        }

        // 🔹 Mostrar texto letra por letra con cursor fijo "_"
        foreach (char ch in currentFullLine)
        {
            activeText.text += ch + "_"; // agregar letra + cursor
            yield return new WaitForSeconds(typingTime);

            // quitar cursor antes de siguiente letra
            activeText.text = activeText.text.TrimEnd('_');
        }

        // Texto final completo antes del cursor titilante
        activeText.text = currentFullLine;

         // Si la línea que terminó de escribirse es del personaje, ponemos animación pestañar
        if (esDialogoRespuesta || (lineIndex % 2 == 0))
        {
            GameManager.instance.ReproducirAnimacionPestañar();
        }
               AudioManager.instance.vozGuardia.Stop();
        AudioManager.instance.vozPersonaje.Stop();

        isTyping = false;
        activeButton.gameObject.SetActive(true);


        // Iniciar parpadeo
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkCursor(activeText, currentFullLine));
    }

    private void FinalizarDialogo()
    {
        AudioManager.instance.vozGuardia.Stop();
        AudioManager.instance.vozPersonaje.Stop();

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



    private IEnumerator BlinkCursor(TMP_Text activeText, string fullLine)
    {
        bool cursorVisible = true;
        float lastTime = Time.time;

        while (didDialogueStart && lineIndex < dialogueLines.Length && !isTyping)
        {
            if (Time.time - lastTime >= 0.5f)
            {
                cursorVisible = !cursorVisible;
                lastTime = Time.time;
            }

            activeText.text = fullLine + (cursorVisible ? "_" : "");
            yield return null;
        }
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

        if (!esDialogoRespuesta)
        {
            dialogosOmitidos++;
            GameManager.instance.dialogosOmitidos++;
            Debug.Log("Diálogo inicial omitido");
        }

        StopAllCoroutines();
        isTyping = false;
        lineIndex = dialogueLines.Length;

        // Volver a la animación de pestañar al omitir diálogo
        GameManager.instance.ReproducirAnimacionPestañar();

  // Reproducir voz personaje
        AudioManager.instance.vozGuardia.Stop();
        AudioManager.instance.vozPersonaje.Stop();
        FinalizarDialogo();
    }

    public bool HaTerminadoElDialogo()
    {
        return !didDialogueStart;
    }
}
