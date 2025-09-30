using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("Referencias")]
    public SimpleDialogueManager simpleDialogueManager; // El gestor de diálogos genérico
    public GameObject botonContinuar; // Botón para pasar a la siguiente escena

    [Header("Paneles y Control")]
    public GameObject controlPanel; // Panel del control remoto
    public GameObject tvPanel;      // Panel de la tele donde se muestran las noticias

    [Header("Overlay Día")]
    public GameObject overlayPanel;     // Panel negro que cubre todo
    public Image overlayBackground;     // Fondo negro del overlay
    public TMP_Text diaText;            // Texto que dice "Día X"
    public float fadeDuration = 2f;     // Duración del fade

    [Header("Noticias por día")]
    [TextArea(2, 5)] public string[] noticiasDia1;
    [TextArea(2, 5)] public string[] noticiasDia2;
    [TextArea(2, 5)] public string[] noticiasDia3;

    private bool isControlPanelActive = false;

    private void Start()
    {
        if (botonContinuar != null)
            botonContinuar.SetActive(false);

        controlPanel.SetActive(false);
        tvPanel.SetActive(false);

        // Mostrar overlay con "Día X"
        if (overlayPanel != null)
        {
            int dia = GameData.NivelActual;
            diaText.text = "DÍA " + dia;
            overlayPanel.SetActive(true);

            // Arrancar fade
            StartCoroutine(FadeOutOverlay());
        }

        // Configuración de noticias (las dejo igual que antes)
        noticiasDia1 = new string[]
        {
            "Buenos días, son las ocho en punto y damos inicio a nuestro boletín informativo",
            "Las autoridades sanitarias han compartido nuevos datos sobre el virus que mantiene en alerta a todo el país",
            "En las últimas horas, médicos de distintas provincias reportaron la aparición de un síntoma particular en los pacientes infectados",
            "Se trata de un sarpullido repentino que suele comenzar en brazos y cuello, y que en cuestión de horas puede extenderse a otras zonas del cuerpo",
            "Aunque la investigación continúa en desarrollo, los especialistas piden a la población estar atenta ante la presencia de estas erupciones cutáneas",
            "El Ministerio de Salud recomienda mantener distancia y evitar el contacto directo con personas que presenten estas marcas en la piel",
            "Continuaremos ampliando esta información a lo largo de la jornada"
        };

        noticiasDia2 = new string[]
        {
            "Buenos días, son las ocho en punto y damos inicio a nuestro boletín informativo",
            "En la última jornada se registraron más casos vinculados al virus y los médicos han detectado un nuevo patrón en los síntomas",
            "Según los reportes, muchos de los pacientes presentan enrojecimiento ocular acompañado de una fuerte irritación",
            "Este síntoma suele aparecer de manera repentina, incluso en personas que no tenían antecedentes de problemas en la vista",
            "Las autoridades sanitarias señalan que la combinación de sarpullido y ojos rojos debe considerarse una señal de alerta temprana",
            "Reiteran además la importancia de mantener distancia con quienes presenten estas características, dado el alto nivel de contagio",
            "Ampliamos esta información a lo largo del día con las declaraciones de especialistas"
        };

        noticiasDia3 = new string[]
        {
            "Buenos días, son las ocho en punto y comenzamos con la actualización de la situación sanitaria",
            "El número de casos continúa en aumento y los especialistas advierten sobre la aparición de un nuevo síntoma en los infectados",
            "Además del sarpullido y el enrojecimiento ocular, se ha registrado un marcado estado de palidez en gran parte de los pacientes",
            "Este síntoma suele manifestarse junto a un cansancio generalizado y una sensación de debilidad progresiva",
            "Los médicos explican que la palidez se debe a la alteración en la circulación sanguínea provocada por el virus",
            "Las autoridades insisten en extremar las medidas de precaución y notificar de inmediato cualquier caso sospechoso",
            "Seguiremos de cerca la evolución de esta información y los mantendremos actualizados durante el día"
        };
    }

    private IEnumerator FadeOutOverlay()
    {
        float t = 0;
        Color bgColor = overlayBackground.color;
        Color textColor = diaText.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);

            overlayBackground.color = new Color(bgColor.r, bgColor.g, bgColor.b, alpha);
            diaText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);

            yield return null;
        }

        overlayPanel.SetActive(false);
    }

    public void CerrarPanelControl()
    {
        controlPanel.SetActive(false);
    }

    public void OnClickRemote()
    {
        isControlPanelActive = !isControlPanelActive;
        controlPanel.SetActive(isControlPanelActive);
    }

    public void PowerTele()
    {
        tvPanel.SetActive(true);
        controlPanel.SetActive(false);
        isControlPanelActive = false;

        string[] noticias = GetNoticiasPorDia(GameData.NivelActual);
        if (noticias != null && noticias.Length > 0)
        {
            simpleDialogueManager.StartDialogue(noticias, OnNoticiasTerminadas);
        }
        else
        {
            Debug.LogWarning("No hay noticias configuradas para el día " + GameData.NivelActual);
            OnNoticiasTerminadas();
        }
    }

    private string[] GetNoticiasPorDia(int dia)
    {
        switch (dia)
        {
            case 1: return noticiasDia1;
            case 2: return noticiasDia2;
            case 3: return noticiasDia3;
            default: return new string[] { "No hay noticias disponibles." };
        }
    }

    private void OnNoticiasTerminadas()
    {
        tvPanel.SetActive(false);
        if (botonContinuar != null)
        {
            botonContinuar.SetActive(true);

            botonContinuar.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                FindObjectOfType<SceneTransition>().LoadScene("Gameplay");
            });
        }
    }
}
