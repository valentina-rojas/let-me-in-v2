using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoticieroAnim : MonoBehaviour
{
    public Image targetImage;          // El componente Image del noticiero
    public Sprite[] frames;            // Frames de la animación
    public float frameRate = 0.1f;     // Tiempo entre cada frame
    public bool loop = true;           // ¿La animación se repite?

    private bool isPlaying = false;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    public void PlayAnim()
    {
        if (!isPlaying && frames.Length > 0)
            StartCoroutine(PlayCoroutine());
    }

    public void StopAnim()
    {
        isPlaying = false;
        StopAllCoroutines();
    }

    private IEnumerator PlayCoroutine()
    {
        isPlaying = true;

        do
        {
            for (int i = 0; i < frames.Length; i++)
            {
                targetImage.sprite = frames[i];
                yield return new WaitForSeconds(frameRate);

                if (!isPlaying) break;
            }
        } while (loop && isPlaying);

        isPlaying = false;
    }
}
