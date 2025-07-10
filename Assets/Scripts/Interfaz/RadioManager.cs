using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RadioManager : MonoBehaviour
{
    public AudioSource ruidosDisturbios;
    public AudioSource audioSeguridad;
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public List<string> mensajesDisturbios = new List<string>
{
    "Un infectado generó disturbios, más cuidado con quien dejas entrar.",
    "Se reportan más incidentes dentro del búnker, la situación está empeorando.",
    "Otro infectado ha causado problemas, la seguridad está en alerta.",
    "El número de infectados causando disturbios está aumentando rápidamente.",
    "Se detectó otro brote de violencia dentro del búnker, ¡Estamos en estado de emergencia!"
};
    public float velocidadEscritura = 0.05f;
    public float duracionMensaje = 5f;

    private bool estaEscribiendo = false;

    public Animator radioAnimator;

    private int indiceMensajeActual = 0;

    public AudioSource loadingBar;
    public AudioSource sonidoEstatica;

    public CharacterSpawn characterSpawn;

    public void ActivarDisturbios()
    {
        StartCoroutine(ActivarDisturbiosCoroutine());
    }

    public IEnumerator ActivarDisturbiosCoroutine()
    {

        yield return new WaitForSeconds(3f);

        //ActualizarContaminacion(1);

        if (radioAnimator != null)
        {
            radioAnimator.SetTrigger("ActivarRadio");
            Debug.Log("Animación ActivarRadio desencadenada");
        }
        else
        {
            Debug.LogWarning("radioAnimator es nulo");
        }
        ruidosDisturbios.Play();
        audioSeguridad.Play();

        panelDialogo.SetActive(true);
        StartCoroutine(EscribirTexto(mensajesDisturbios[indiceMensajeActual]));


        while (estaEscribiendo)
        {
            yield return null;
        }

        yield return new WaitForSeconds(duracionMensaje);

        panelDialogo.SetActive(false);

        audioSeguridad.Stop();

        yield return new WaitForSeconds(4f);

        ruidosDisturbios.Stop();


        if (radioAnimator != null)
        {
            radioAnimator.SetTrigger("IdleRadio");
        }

        indiceMensajeActual = (indiceMensajeActual + 1) % mensajesDisturbios.Count;

        yield return new WaitForSeconds(1f);

        characterSpawn.FinalizarInteraccion();
    }


    private IEnumerator EscribirTexto(string mensaje)
    {
        estaEscribiendo = true;
        textoDialogo.text = "";

        foreach (char letra in mensaje.ToCharArray())
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        estaEscribiendo = false;
    }





    public void PantallaRadio()
    {

        StartCoroutine(ActivarPantallaRadio());

    }

    public IEnumerator ActivarPantallaRadio()
    {
        radioAnimator.SetTrigger("ActivarRadio");
        sonidoEstatica.Play();
        yield return new WaitForSeconds(2f);
        sonidoEstatica.Stop();
        radioAnimator.SetTrigger("IdleRadio");
    }


}
