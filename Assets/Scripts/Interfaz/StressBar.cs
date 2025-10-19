using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{
    [Header("Barra de estrés")]
    public Slider barraEstres;
    public float maxEstres = 3f;
    private float nivelEstres = 0f;
    public Image fillBarImage;
    public GameObject panelPerdiste;

    [Header("Colores de la barra de estrés")]
    public Color colorVerdeClaro;
    public Color colorVerdeOscuro;
    public Color colorAmarillo;
    public Color colorNaranja;
    public Color colorRojo;

    [Header("Animación del mate")]
    public Image mateImage;                 // Componente Image del mate
    public List<Sprite> framesAnimacion;    // Frames de animación desde el inspector
    public float velocidadFrames = 0.1f;    // Tiempo entre frames
    public Sprite spriteIdle;               // Sprite por defecto
    public AudioSource sonidoMate;          // Sonido durante la animación

    [Header("Sonidos adicionales")]
    public AudioSource loadingBar;

    private bool animando = false;

    void Start()
    {
        if (barraEstres != null)
        {
            barraEstres.maxValue = maxEstres;
            barraEstres.value = nivelEstres;
        }

        if (mateImage != null && spriteIdle != null)
            mateImage.sprite = spriteIdle;
    }

    public void ActualizarEstres(float cantidad)
    {
        nivelEstres += cantidad;

        if (nivelEstres > maxEstres)
            nivelEstres = maxEstres;

        if (barraEstres != null)
        {
            barraEstres.value = nivelEstres;
            ActualizarColorBarra();
            AudioManager.instance.sonidoLoading.Play();
        }

        if (nivelEstres >= maxEstres)
            PerderJuego();
    }

    public void DisminuirEstres(float cantidad)
    {
        nivelEstres -= cantidad;

        // Reproducir animación del mate
        if (!animando)
            StartCoroutine(AnimacionMate());

        if (nivelEstres < 0)
            nivelEstres = 0;

        if (barraEstres != null)
        {
            barraEstres.value = nivelEstres;
            ActualizarColorBarra();
            AudioManager.instance.sonidoLoading.Play();
        }
    }

    private IEnumerator AnimacionMate()
    {
        animando = true;

        if (sonidoMate != null)
            sonidoMate.Play();

        for (int i = 0; i < framesAnimacion.Count; i++)
        {
            mateImage.sprite = framesAnimacion[i];
            yield return new WaitForSeconds(velocidadFrames);
        }

        // Vuelve al sprite por defecto
        if (spriteIdle != null)
            mateImage.sprite = spriteIdle;

        animando = false;

        if (sonidoMate != null)
            sonidoMate.Stop();
    }

    private void ActualizarColorBarra()
    {
        if (nivelEstres <= 0f)
            fillBarImage.color = colorVerdeClaro;
        else if (nivelEstres == 1f)
            fillBarImage.color = colorVerdeOscuro;
        else if (nivelEstres == 2f)
            fillBarImage.color = colorAmarillo;
        else if (nivelEstres == 3f)
            fillBarImage.color = colorRojo;
    }

    public void PerderJuego()
    {
        GameManager.instance.GameOver(GameManager.TipoDerrota.Estres);
    }

    // Si querés que se anime al clickear el mate directamente desde el UI
    public void OnClickMate()
    {
        if (!animando)
            StartCoroutine(AnimacionMate());
    }
}
