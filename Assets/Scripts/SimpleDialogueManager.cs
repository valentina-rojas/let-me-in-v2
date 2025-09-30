using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SimpleDialogueManager : MonoBehaviour
{
    [SerializeField] private float typingTime = 0.02f;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button botonSiguiente;

    private string[] dialogueLines;
    private int lineIndex;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullLine = "";
    private Coroutine blinkCoroutine;
    private System.Action onDialogueFinished;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        botonSiguiente.onClick.AddListener(NextDialogueLine);
    }

    public void StartDialogue(string[] lines, System.Action onFinished = null)
    {
        dialogueLines = lines;
        lineIndex = 0;
        dialoguePanel.SetActive(true);
        onDialogueFinished = onFinished;
        typingCoroutine = StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        if (isTyping)
        {
            // Mostrar línea completa si se adelanta
            StopCoroutine(typingCoroutine);
            isTyping = false;
            dialogueText.text = currentFullLine;

            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkCursor(currentFullLine));
            return;
        }

        lineIndex++;

        if (lineIndex < dialogueLines.Length)
        {
            typingCoroutine = StartCoroutine(ShowLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator ShowLine()
    {
        isTyping = true;
        currentFullLine = dialogueLines[lineIndex];
        dialogueText.text = "";
        botonSiguiente.gameObject.SetActive(false);

        foreach (char ch in currentFullLine)
        {
            dialogueText.text += ch + "_";
            yield return new WaitForSeconds(typingTime);
            dialogueText.text = dialogueText.text.TrimEnd('_');
        }

        dialogueText.text = currentFullLine;
        isTyping = false;
        botonSiguiente.gameObject.SetActive(true);

        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkCursor(currentFullLine));
    }

    private IEnumerator BlinkCursor(string fullLine)
    {
        bool visible = true;
        float lastTime = Time.time;

        while (!isTyping && lineIndex < dialogueLines.Length)
        {
            if (Time.time - lastTime >= 0.5f)
            {
                visible = !visible;
                lastTime = Time.time;
            }

            dialogueText.text = fullLine + (visible ? "_" : "");
            yield return null;
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        if (onDialogueFinished != null) onDialogueFinished.Invoke();
    }
}
