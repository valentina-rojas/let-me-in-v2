using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voicesSlider;

    public AudioMixer audioMixer;

    public RectTransform panelOpciones;
    public RectTransform panelAyuda;
    public RectTransform panelSintomas;
    public RectTransform[] panelesSintomas;

    public Button botonOpciones;
    public Button botonAyuda;
    public Button botonSintomas;

    public InteractableObjects interactableObjects;
    public Zoom zoomManager;

    public AudioSource desplegarpestañas;

    private void Start()
    {
        // Cargar valores guardados (o usar 0.5f por defecto)
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        voicesSlider.value = PlayerPrefs.GetFloat("VoicesVolume", 0.5f);

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetVoicesVolume(voicesSlider.value);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        voicesSlider.onValueChanged.AddListener(SetVoicesVolume);
    }

    public void AbrirOpciones()
    {
        AudioManager.instance.sonidoDesplegarPestañas.Play();
        CerrarTodosLosPaneles();
        panelOpciones.gameObject.SetActive(true);
    }

    public void CerrarOpciones()
    {
        AudioManager.instance.sonidoDesplegarPestañas.Play();
        panelOpciones.gameObject.SetActive(false);
        interactableObjects.ActivarEventTriggers();
    }

    public void SetMusicVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetVoicesVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("VoicesVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
        PlayerPrefs.SetFloat("VoicesVolume", volume);
    }

    public void AbrirAyuda()
    {
        AudioManager.instance.sonidoDesplegarPestañas.Play();
        CerrarTodosLosPaneles();
        panelAyuda.gameObject.SetActive(true);
    }

    public void CerrarAyuda()
    {
        AudioManager.instance.sonidoDesplegarPestañas.Play();
        panelAyuda.gameObject.SetActive(false);
        interactableObjects.ActivarEventTriggers();
    }

    public void AbrirSintomas()
    {
        AudioManager.instance.sonidoDesplegarPestañas.Play();
        CerrarTodosLosPaneles();
        panelSintomas.gameObject.SetActive(true);

        foreach (var panel in panelesSintomas)
            panel.gameObject.SetActive(false);

        int indice = GameData.NivelActual - 1;
        if (indice >= 0 && indice < panelesSintomas.Length)
            panelesSintomas[indice].gameObject.SetActive(true);
    }

    public void CerrarSintomas()
    {
        AudioManager.instance.sonidoDesplegarPestañas.Play();
        panelSintomas.gameObject.SetActive(false);
        interactableObjects.ActivarEventTriggers();
    }

    public void CerrarTodosLosPaneles()
    {
        interactableObjects.DesactivarEventTriggers();

        if (zoomManager != null && zoomManager.isLupaVisible)
            zoomManager.ToggleLupa();

        panelOpciones.gameObject.SetActive(false);
        panelAyuda.gameObject.SetActive(false);
        panelSintomas.gameObject.SetActive(false);
    }

    public void DesactivarBotonesVentanas()
    {
        interactableObjects.DesactivarEventTriggers();

        if (zoomManager != null && zoomManager.isLupaVisible)
            zoomManager.ToggleLupa();

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
