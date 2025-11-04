using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LupaInteractiva : MonoBehaviour
{
    [Header("Panel de Lupa")]
    public GameObject panelLupa;

    [Header("Imagen a mostrar")]
    public Image imagenZona;

    [Header("Botón de abrir lupa")]
    public Button botonAbrirLupa;

    [Header("Botones de zonas")]
    public Button botonOjos;
    public Button botonBoca;
    public Button botonCuello;
    public Button botonEspalda;

    [Header("Energía de lupa")]
    public List<Image> unidadesEnergia; // imágenes de energía
    private int energiaActual;

    [Header("UI Sin Energía / Confirmación")]
    public GameObject panelSinEnergia;
    public GameObject panelConfirmacion;
    public TMP_Text textoConfirmacion;
    public Button botonConfirmar;
    public Button botonCancelar;

    private CharacterAttributes personajeActual;
    private string zonaSeleccionadaPendiente;
    private System.Action accionPendiente;

    void Start()
    {
        panelLupa.SetActive(false);
        panelSinEnergia.SetActive(false);
        panelConfirmacion.SetActive(false);
        botonAbrirLupa.interactable = false;

        energiaActual = unidadesEnergia.Count;

        botonAbrirLupa.onClick.AddListener(AbrirCerrarLupa);

        botonOjos.onClick.AddListener(() => SolicitarConfirmacion("los ojos", personajeActual?.lupaOjos));
        botonBoca.onClick.AddListener(() => SolicitarConfirmacion("la boca", personajeActual?.lupaBoca));
        botonCuello.onClick.AddListener(() => SolicitarConfirmacion("el cuello", personajeActual?.lupaCuello));
        botonEspalda.onClick.AddListener(() => SolicitarConfirmacion("la espalda", personajeActual?.lupaEspalda));

        botonConfirmar.onClick.AddListener(ConfirmarAccion);
        botonCancelar.onClick.AddListener(CancelarAccion);
    }

    void AbrirCerrarLupa()
    {
        panelLupa.SetActive(!panelLupa.activeSelf);

        if (panelLupa.activeSelf)
        {
            personajeActual = GameManager.instance.personajeActual;

            if (personajeActual != null && personajeActual.lupaDefault != null)
                imagenZona.sprite = personajeActual.lupaDefault;

            ActualizarEstadoBotones();
        }
    }

    // Muestra ventana de confirmación antes de usar energía
    void SolicitarConfirmacion(string nombreZona, Sprite zonaSprite)
    {
        zonaSeleccionadaPendiente = nombreZona;
        accionPendiente = () => MostrarZona(zonaSprite);

        textoConfirmacion.text = $"¿Está seguro que desea examinar {nombreZona}? Consumirá una unidad de energía.";
        panelConfirmacion.SetActive(true);
    }

    void ConfirmarAccion()
    {
        panelConfirmacion.SetActive(false);
        if (accionPendiente != null)
        {
            accionPendiente.Invoke();
            ConsumirEnergia();
            accionPendiente = null;
        }
    }

    void CancelarAccion()
    {
        panelConfirmacion.SetActive(false);
        accionPendiente = null;
    }

    void MostrarZona(Sprite zona)
    {
        if (zona != null)
        {
            imagenZona.sprite = zona;
        }
        else
        {
            Debug.LogWarning("No hay sprite asignado para esta zona del personaje.");
        }
    }

    void ConsumirEnergia()
    {
        if (energiaActual > 0)
        {
            energiaActual--;
            unidadesEnergia[energiaActual].enabled = false;

            if (energiaActual <= 0)
                ActualizarEstadoBotones();
        }
    }

    void ActualizarEstadoBotones()
    {
        bool tieneEnergia = energiaActual > 0;

        botonOjos.interactable = tieneEnergia;
        botonBoca.interactable = tieneEnergia;
        botonCuello.interactable = tieneEnergia;
        botonEspalda.interactable = tieneEnergia;

        if (!tieneEnergia)
            MostrarPanelSinEnergia(true); // mostrar solo el botón, no el panel completo
    }

    void MostrarPanelSinEnergia(bool mostrarMensaje = true)
    {

            panelSinEnergia.SetActive(true);
    }

    public void DesactivarBotonLupa()
    {
        botonAbrirLupa.interactable = false;
    }

    public void ActivarBotonLupa()
    {
        botonAbrirLupa.interactable = true;
    }

    public void ReiniciarEnergia()
    {
        energiaActual = unidadesEnergia.Count;
        foreach (var unidad in unidadesEnergia)
            unidad.enabled = true;

        panelSinEnergia.SetActive(false);
        panelConfirmacion.SetActive(false);
    }
}
