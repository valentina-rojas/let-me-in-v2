using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{

  [Header("Diálogo")]
  [SerializeField] private GameObject dialoguePanelPersonaje;
  [SerializeField] private GameObject dialoguePanelGuardia;
  [SerializeField] private TMP_Text dialogueTextPersonaje;
  [SerializeField] private TMP_Text dialogueTextGuardia;
  [SerializeField] private Button botonSiguientePersonaje;
  [SerializeField] private Button botonSiguienteGuardia;

  public GameObject GetDialoguePanelPersonaje() => dialoguePanelPersonaje;
  public GameObject GetDialoguePanelGuardia() => dialoguePanelGuardia;
  public TMP_Text GetDialogueTextPersonaje() => dialogueTextPersonaje;
  public TMP_Text GetDialogueTextGuardia() => dialogueTextGuardia;
  public Button GetBotonSiguientePersonaje() => botonSiguientePersonaje;
  public Button GetBotonSiguienteGuardia() => botonSiguienteGuardia;

  [Header("Mensaje Inicio Nivel")]
  public RectTransform panelInicioDia;
  public TextMeshProUGUI textoInicioDia;
  public Image imagenInicioDia;
  public Sprite[] imagenesNiveles;

  [Header("Reporte Fin Nivel")]
  public RectTransform panelReporte;
  public TextMeshProUGUI mensajeReporte;
  public TextMeshProUGUI reporteText;

  [Header("Paneles de derrota")]
  public GameObject panelPerdisteEstres;
  public GameObject panelPerdisteDespido;
  public GameObject panelPerdisteDisturbios;
  public GameObject panelGanaste;

  [Header("Indicaciones Primer Nivel")]
  public RectTransform indicaciones1;
  public RectTransform indicaciones2;
  public RectTransform indicaciones3;
  public RectTransform indicaciones4;
  public RectTransform indicaciones5;
  public TextMeshProUGUI omitirIndicaciones;
  private Coroutine panelInicioDiaCoroutine;

  [Header("Botones de los paneles")]
  public Button botonSiguienteNivel;
  public Button botonGanaste;
  public Button botonPerdiste;
  public Button botonPerdisteDisturbios;

  [Header("Managers y sistemas")]
  public DialogueManager dialogueManager;
  public CharacterManager characterManager;
  public OptionsManager optionsManager;
  public InteractableObjects interactableObjects;

  public event Action PanelInicioDesactivado;

  public float duracionIndicaciones = 4f;
  public float velocidadTexto = 0.1f;
  public float duracionPanel = 1.0f;
  public float intervaloCursor = 0.5f;
  private float tiempoUltimaActualizacion;

  private bool cursorVisible = true;

  public TextMeshProUGUI contadorPersonas;

  public void MostrarInicioDia(string mensaje)
  {

    interactableObjects.DesactivarEventTriggers();

    int nivelActual = GameManager.instance.NivelActual - 1; // Si el nivel empieza en 1, resta 1 para usarlo como índice

    if (nivelActual >= 0 && nivelActual < imagenesNiveles.Length)
    {
      imagenInicioDia.sprite = imagenesNiveles[nivelActual];  // Asigna la imagen correspondiente al nivel actual
    }
    else
    {
      Debug.LogWarning("No hay imagen asignada para este nivel.");
    }

    panelReporte.gameObject.SetActive(false);
    // panelPerdiste.gameObject.SetActive(false);
    botonSiguienteNivel.gameObject.SetActive(false);

    panelInicioDia.gameObject.SetActive(true);
    int diaActual = GameManager.instance.NivelActual;
    string titulo = $"Día {diaActual}\n\n";

    panelInicioDiaCoroutine = StartCoroutine(MostrarPanelInicioDiaCoroutine(titulo + mensaje));

    PanelInicioDesactivado += MostrarPanelIndicaciones;
  }
public void MostrarPanelIndicaciones()
{
    if (GameManager.instance.NivelActual == 1)
    {
        optionsManager.AbrirAyuda();
        StartCoroutine(MostrarIndicacionesSecuencia()); // ahora lo maneja la corrutina
    }
    else
    {
        IniciarJuego();
        interactableObjects.ActivarEventTriggers();
    }
}

private IEnumerator MostrarIndicacionesSecuencia()
{
    yield return null; // espera un frame para que el panel se active visualmente
    yield return new WaitUntil(() => !optionsManager.panelAyuda.gameObject.activeSelf);

    IniciarJuego();
    interactableObjects.ActivarEventTriggers();
}

  public IEnumerator MostrarPanelInicioDiaCoroutine(string mensaje)
  {
    textoInicioDia.text = "";

    AudioManager.instance.sonidoTecleo.Play();

    tiempoUltimaActualizacion = Time.time;

    // Mostrar el texto letra por letra
    for (int i = 0; i < mensaje.Length; i++)
    {
      textoInicioDia.text += mensaje[i]; // Añadir letra
      textoInicioDia.text += "_"; // Añadir el cursor

      yield return new WaitForSeconds(velocidadTexto);
      textoInicioDia.text = textoInicioDia.text.TrimEnd('_'); // Quitar el cursor en la siguiente iteración
    }

    // Finalizar texto
    textoInicioDia.text = mensaje;

    AudioManager.instance.sonidoTecleo.Stop();

    // Mostrar el texto completo con el cursor titilante
    while (true)
    {
      if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
      {
        cursorVisible = !cursorVisible;
        tiempoUltimaActualizacion = Time.time;
      }

      string textoConCursorTitilante = mensaje;
      if (cursorVisible)
      {
        textoConCursorTitilante += "_";
      }
      textoInicioDia.text = textoConCursorTitilante;

      yield return null; // Esperar al siguiente frame

    }
  }



  public void CerrarPanelInicioDia()
  {
    AudioManager.instance.sonidoDesplegarPestañas.Play();

    if (panelInicioDiaCoroutine != null)
    {
      StopCoroutine(panelInicioDiaCoroutine);
      panelInicioDiaCoroutine = null;
    }

    AudioManager.instance.sonidoTecleo.Stop();
    panelInicioDia.gameObject.SetActive(false);

    optionsManager.ActivarBotonesVentanas();

    //llamar iniico juego
    PanelInicioDesactivado?.Invoke();
  }


  public void ActualizarPanelReporte(int sanosIngresados, int enfermosIngresados, int sanosRechazados, int enfermosRechazados)
  {
    panelReporte.gameObject.SetActive(true);
    // botonSiguienteNivel.gameObject.SetActive(true);

    optionsManager.DesactivarBotonesVentanas();
    interactableObjects.DesactivarEventTriggers();

    int diaActual = GameManager.instance.NivelActual;
    string tituloReporte = $"Reporte Día {diaActual}\n";
    reporteText.text = $"{tituloReporte}" +
                    $"Sanos ingresados: {sanosIngresados}\n" +
                    $"Enfermos ingresados: {enfermosIngresados}\n" +
                    $"Sanos rechazados: {sanosRechazados}\n" +
                    $"Enfermos rechazados: {enfermosRechazados}";
    GameManager.instance.MostrarMensaje();
  }

  public void IniciarJuego()
  {
    GameManager.instance.IniciarSpawnDePersonajes();
  }

  public void MostrarPantallaDerrota(GameManager.TipoDerrota tipo)
  {
    switch (tipo)
    {
      case GameManager.TipoDerrota.Estres:
        panelPerdisteEstres.SetActive(true);
        break;
      case GameManager.TipoDerrota.Despido:
        panelPerdisteDespido.SetActive(true);
        break;
      case GameManager.TipoDerrota.Disturbios:
        panelPerdisteDisturbios.SetActive(true);
        break;
    }
  }

  public void ActualizarContadorPersonas(int cantidadRestante)
  {
    if (contadorPersonas != null)
    {
      contadorPersonas.text = cantidadRestante.ToString();
    }
  }

  public void OnBotonSiguienteNivelClick()
  {
    panelReporte.gameObject.SetActive(false);
    GameManager.instance.SiguienteNivel();
  }


  public void ActivarPanelGanaste()
  {
    panelGanaste.SetActive(true);
  }

}