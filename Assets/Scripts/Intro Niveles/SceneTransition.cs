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
            DontDestroyOnLoad(gameObject); // persiste entre escenas
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
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
