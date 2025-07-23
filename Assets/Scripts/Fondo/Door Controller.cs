using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public AudioSource puertaAbriendose;
    public Transform capaPuerta;
    public float alturaMovimiento = 2f;
    public float tiempoMovimiento = 1f;


    public IEnumerator Abrir()
    {
        if (puertaAbriendose != null) puertaAbriendose.Play();

        Vector3 posicionInicial = capaPuerta.position;
        Vector3 posicionFinal = new Vector3(posicionInicial.x, posicionInicial.y + alturaMovimiento, posicionInicial.z);

        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < tiempoMovimiento)
        {
            capaPuerta.position = Vector3.Lerp(posicionInicial, posicionFinal, tiempoTranscurrido / tiempoMovimiento);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        capaPuerta.position = posicionFinal;
    }

    public IEnumerator Cerrar()
    {
        Vector3 posicionFinal = capaPuerta.position;
        Vector3 posicionInicial = new Vector3(posicionFinal.x, posicionFinal.y - alturaMovimiento, posicionFinal.z);

        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < tiempoMovimiento)
        {
            capaPuerta.position = Vector3.Lerp(posicionFinal, posicionInicial, tiempoTranscurrido / tiempoMovimiento);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        capaPuerta.position = posicionInicial;

        if (puertaAbriendose != null && puertaAbriendose.isPlaying)
            puertaAbriendose.Stop();
    }
}
