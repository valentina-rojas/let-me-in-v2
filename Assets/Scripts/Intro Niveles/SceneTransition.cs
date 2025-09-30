using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // el objeto sobrevive a los cambios de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Asegurar que arranque invisible
        if (fadePanel != null)
            fadePanel.alpha = 0;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

     private IEnumerator FadeOutAndLoad(string sceneName)
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("⚠ fadePanel no asignado o fue destruido antes del fade.");
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        float t = 0f;
        while (t < fadeDuration)
        {
            if (fadePanel == null) yield break; // seguridad
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
