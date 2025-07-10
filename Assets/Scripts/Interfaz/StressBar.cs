using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StressBar : MonoBehaviour
{
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

    public Animator mate;
    public AudioSource audioMate;
    public AudioSource loadingBar;

    void Start()
    {

        if (barraEstres != null)
        {
            barraEstres.maxValue = maxEstres;
            barraEstres.value = nivelEstres;
        }
    }


    public void ActualizarEstres(float cantidad)
    {
        nivelEstres += cantidad;

        if (nivelEstres > maxEstres)
        {
            nivelEstres = maxEstres;
        }

        if (barraEstres != null)
        {
            barraEstres.value = nivelEstres;
            ActualizarColorBarra();
         //   loadingBar.Play();
        }

        if (nivelEstres >= maxEstres)
        {
            PerderJuego();
        }
    }



    public void DisminuirEstres(float cantidad)
    {
        nivelEstres -= cantidad;

        StartCoroutine(AnimacionMate());

        if (nivelEstres < 0)
        {
            nivelEstres = 0;
        }


        if (barraEstres != null)
        {
            barraEstres.value = nivelEstres;
            ActualizarColorBarra();
        }
    }


    public IEnumerator AnimacionMate()
    {
        mate.SetTrigger("mateSpin");
      //  audioMate.Play();
        yield return new WaitForSeconds(2f);
      //  audioMate.Stop();
        mate.SetTrigger("mateIdle");
    }


  private void ActualizarColorBarra()
{
    if (nivelEstres <= 0f)
    {
        fillBarImage.color = colorVerdeClaro;
    }
    else if (nivelEstres == 1f)
    {
        fillBarImage.color = colorVerdeOscuro;
    }
    else if (nivelEstres == 2f)
    {
        fillBarImage.color = colorAmarillo;
    }
    else if (nivelEstres == 3f)
    {
        fillBarImage.color = colorRojo;
    }
}



    public void PerderJuego()
    {
        GameManager.instance.GameOver(GameManager.TipoDerrota.Estres);
    }

}
