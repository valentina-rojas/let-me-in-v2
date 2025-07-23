using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource sonidoPuerta;
    public AudioSource sonidoPasosGuardia;
    public AudioSource sonidoEscoba;
    public AudioSource sonidoPasosPersonaje;
    public AudioSource sonidoTecleo;
    public AudioSource sonidoDisparo;
    public AudioSource sonidoPalanca;
    public AudioSource sonidoBotonPresionado;
    public AudioSource sonidoPapelesMoviendose;
    public AudioSource sonidoEstaticaRadio;
    public AudioSource sonidoMateGirando;
    public AudioSource sonidoLoading;
    public AudioSource sonidoMovimientoEscaner;
    public AudioSource sonidoBeepEscaner;
    public AudioSource sonidoLuzEscaner;
    public AudioSource sonidoDesplegarPestañas;
    public AudioSource sonidoDisturbios;
    public AudioSource sonidoPeligro;

    public AudioSource vozGuardia;
    public AudioSource vozPersonaje;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
