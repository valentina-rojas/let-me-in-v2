using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class OptionsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voicesSlider;

    public AudioMixer audioMixer;

    public RectTransform panelOpciones;
    public RectTransform panelAyuda;
    public RectTransform panelSintomas;

    public RectTransform panelSintomasNivel1;
    public RectTransform panelSintomasNivel2;

    public Button botonOpciones;
    public Button botonAyuda;
    public Button botonSintomas;


    public InteractableObjects interactableObjects;
    public Zoom zoomManager;

    public AudioSource desplegarpestañas;

    void Start()
    {
        musicSlider.value = 0.5f;
        sfxSlider.value = 0.5f;
        voicesSlider.value = 0.5f;

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetVoicesVolume(voicesSlider.value);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        voicesSlider.onValueChanged.AddListener(SetVoicesVolume);
    }

    public void AbrirOpciones()
    {
        desplegarpestañas.Play();
        CerrarTodosLosPaneles();
        panelOpciones.gameObject.SetActive(true);
        //  Time.timeScale = 0f;
    }

    public void CerrarOpciones()
    {
         desplegarpestañas.Play();
        panelOpciones.gameObject.SetActive(false);

        interactableObjects.ActivarEventTriggers();

    }

    public void SetMusicVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
    }

    public void SetVoicesVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("VoicesVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
    }

    public void AbrirAyuda()
    {
         desplegarpestañas.Play();
        CerrarTodosLosPaneles();
        panelAyuda.gameObject.SetActive(true);
    }

    public void CerrarAyuda()
    {
         desplegarpestañas.Play();
        panelAyuda.gameObject.SetActive(false);

        interactableObjects.ActivarEventTriggers();

    }

    public void AbrirSintomas()
    {
         desplegarpestañas.Play();
        CerrarTodosLosPaneles();
        panelSintomas.gameObject.SetActive(true);

        if (GameData.NivelActual == 1)
        {
            panelSintomasNivel2.gameObject.SetActive(false);
            panelSintomasNivel1.gameObject.SetActive(true);
        }

        if (GameData.NivelActual == 2)
        {
            panelSintomasNivel1.gameObject.SetActive(false);
            panelSintomasNivel2.gameObject.SetActive(true);
        }

    }

    public void CerrarSintomas()
    {
         desplegarpestañas.Play();
        panelSintomas.gameObject.SetActive(false);

        interactableObjects.ActivarEventTriggers();
    }

    // Método para cerrar todos los paneles
    public void CerrarTodosLosPaneles()
    {
        interactableObjects.DesactivarEventTriggers();

        // Desactivar la lupa si está visible
        if (zoomManager != null && zoomManager.isLupaVisible)
        {
            zoomManager.ToggleLupa();
        }

        panelOpciones.gameObject.SetActive(false);
        panelAyuda.gameObject.SetActive(false);
        panelSintomas.gameObject.SetActive(false);
    }

    public void DesactivarBotonesVentanas()
    {
        interactableObjects.DesactivarEventTriggers();

        // Desactivar la lupa si está visible
        if (zoomManager != null && zoomManager.isLupaVisible)
        {
            zoomManager.ToggleLupa();
        }

        botonAyuda.interactable = false;
        botonOpciones.interactable = false;
        botonSintomas.interactable = false;
    }

    public void ActivarBotonesVentanas()
    {

        interactableObjects.ActivarEventTriggers();

        botonAyuda.interactable = true;
        botonOpciones.interactable = true;
        botonSintomas.interactable = true;
    }

}
