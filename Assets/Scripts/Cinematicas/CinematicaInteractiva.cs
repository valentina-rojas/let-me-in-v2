using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CinematicaInteractiva : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPopUpsMails;
    public GameObject panelMailExpandido;
    public GameObject scrollViewMail;
    public GameObject panelTextoMinisterio;
    public CanvasGroup panelFade; 

    [Header("Botones")]
    [SerializeField] private Button botonPopup;
    [SerializeField] private Button botonExpandirMail;
    [SerializeField] private Button botonResponder;
    [SerializeField] private Button botonOpcion1;
    [SerializeField] private Button botonOpcion2;
    [SerializeField] private Button botonFinal; 

    [Header("Textos")]
    public TMP_Text textoJugador;
    public TMP_Text textoMinisterio;

    [Header("Configuración")]
    public float typingSpeed = 0.03f;
    public string proximaEscena = "EscenaSiguiente"; 
    public float fadeDuration = 1.5f;

    private int etapaActual = 1;

    private void Start()
    {
        botonPopup.onClick.AddListener(AbrirPopUps);
        botonExpandirMail.onClick.AddListener(ExpandirMails);
        botonResponder.onClick.AddListener(MostrarOpciones);
        botonFinal.onClick.AddListener(FinalizarCinematica); 

        botonOpcion1.onClick.AddListener(() => SeleccionarRespuesta(1));
        botonOpcion2.onClick.AddListener(() => SeleccionarRespuesta(2));

        botonOpcion1.gameObject.SetActive(false);
        botonOpcion2.gameObject.SetActive(false);
        botonFinal.gameObject.SetActive(false);
        panelTextoMinisterio.SetActive(false);

        if (panelFade != null)
            panelFade.alpha = 0;
    }

    private void AbrirPopUps()
    {
        botonPopup.gameObject.SetActive(false);
        panelPopUpsMails.SetActive(true);
    }

    private void ExpandirMails()
    {
        panelPopUpsMails.SetActive(false);
        panelMailExpandido.SetActive(true);
    }

    private void MostrarOpciones()
    {
        botonResponder.gameObject.SetActive(false);
        textoJugador.text = "";

        switch (etapaActual)
        {
            case 1:
                ActivarBotones(
                    "Hola, buenas noches. No tengo conocimiento de ningún familiar con ese apellido...",
                    "¿Podrían darme más información?"
                );
                break;

            case 2:
                ActivarBotones(
                    "¿Tendría que mudarme al Búnker temporalmente?",
                    "Sí acepto... ¿voy a tener protección asegurada?"
                );
                break;

            case 3:
                ActivarBotones("No me interesa participar en esto.");
                break;

            case 4:
                ActivarBotones("Bueno... En ese caso, acepto el puesto.");
                break;
        }
    }

    private void ActivarBotones(string texto1, string texto2 = null)
    {
        botonOpcion1.GetComponentInChildren<TMP_Text>().text = texto1;
        botonOpcion1.gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(texto2))
        {
            botonOpcion2.GetComponentInChildren<TMP_Text>().text = texto2;
            botonOpcion2.gameObject.SetActive(true);
        }
        else
        {
            botonOpcion2.gameObject.SetActive(false);
        }
    }

    private void SeleccionarRespuesta(int id)
    {
        botonOpcion1.gameObject.SetActive(false);
        botonOpcion2.gameObject.SetActive(false);
        scrollViewMail.SetActive(false);
        panelTextoMinisterio.SetActive(true);

        textoMinisterio.text = "";

        StartCoroutine(EjecutarEtapa(etapaActual, id));
    }

    private IEnumerator EjecutarEtapa(int etapa, int opcion)
    {
        textoJugador.text = "";
        textoMinisterio.text = "";

        string[] jugador = ObtenerLineasJugador(etapa, opcion);
        string[] ministerio = ObtenerLineasMinisterio(etapa, opcion);

        foreach (string linea in jugador)
        {
            yield return StartCoroutine(TipearLinea(textoJugador, linea));
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.4f);

        foreach (string linea in ministerio)
        {
            yield return StartCoroutine(TipearLinea(textoMinisterio, linea));
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.4f);

        etapaActual++;

        // Si hay más etapas → mostrar opciones
        if (etapaActual <= 4)
        {
            MostrarOpciones();
        }
        else
        {
            // 👇 Si terminó el diálogo final del ministerio
            botonFinal.gameObject.SetActive(true);
        }
    }

    private IEnumerator TipearLinea(TMP_Text campoTexto, string linea)
    {
        foreach (char c in linea)
        {
            campoTexto.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        campoTexto.text += "\n";
    }

    // ========= FINAL ========= //
    private void FinalizarCinematica()
    {
        botonFinal.gameObject.SetActive(false);
        StartCoroutine(FadeOutYCambiarEscena());
    }

    private IEnumerator FadeOutYCambiarEscena()
    {
        // Cerrar panel del mail expandido
        panelMailExpandido.SetActive(false);

        if (panelFade != null)
        {
            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                panelFade.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                yield return null;
            }
        }

        SceneManager.LoadScene(proximaEscena);
    }

    // ======= DIALOGOS ======= //
    private string[] ObtenerLineasJugador(int etapa, int opcion)
    {
        switch (etapa)
        {
            case 1:
                if (opcion == 1)
                    return new string[]
                    {
                        "Jugador: Hola, buenas noches. No tengo conocimiento de ningún familiar con ese apellido...",
                        "Jugador: ¿Podrían darme más información?"
                    };
                else
                    return new string[]
                    {
                        "Jugador: Sí acepto... ¿voy a tener protección asegurada?"
                    };

            case 2:
                if (opcion == 1)
                    return new string[]
                    {
                        "Jugador: ¿Tendría que mudarme al Búnker temporalmente?",
                        "Jugador: Sí acepto... ¿voy a tener protección asegurada?"
                    };
                else
                    return new string[]
                    {
                        "Jugador: Sí acepto... ¿voy a tener protección asegurada?"
                    };

            case 3:
                return new string[] { "Jugador: No me interesa participar en esto." };

            case 4:
                return new string[] { "Jugador: Bueno... En ese caso, acepto el puesto." };
        }

        return new string[0];
    }

    private string[] ObtenerLineasMinisterio(int grupo, int opcion)
    {
        switch (grupo)
        {
            case 1:
                if (opcion == 1)
                    return new string[]
                    {
                        "El señor Juan Bizcochuelo fue propietario del Búnker Sector Sur, actualmente bajo custodia del Estado."
                    };
                else
                    return new string[]
                    {
                        "Su nombre figura en los registros de herencia indirecta.",
                        "No se requiere relación familiar directa, solo confirmación de identidad."
                    };

            case 2:
                if (opcion == 1)
                    return new string[]
                    {
                        "Sí. El búnker cuenta con medidas sanitarias, provisiones y alojamiento para el personal.",
                    };
                else
                    return new string[]
                    {
                        "La prioridad de seguridad dependerá de los recursos disponibles."
                    };

            case 3:
                return new string[]
                {
                    "Entendemos su decisión. No obstante, debe considerar que negarse a colaborar podría implicar sanciones administrativas durante el estado de emergencia."
                };

            case 4:
                return new string[]
                {
                    "Agradecemos su colaboración.",
                    "Un vehículo oficial pasará por usted mañana a primera hora para trasladarlo al Búnker Sector Sur.",
                    "Por favor, prepare su documentación y pertenencias personales."
                };

            default:
                return new string[] { "Ministerio: (sin respuesta disponible)" };
        }
    }
}
