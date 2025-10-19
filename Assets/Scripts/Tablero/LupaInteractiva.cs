using UnityEngine;
using UnityEngine.UI;

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

    private CharacterAttributes personajeActual;

    void Start()
    {
        panelLupa.SetActive(false);
        botonAbrirLupa.interactable = false;

        // Botón de abrir/cerrar
        botonAbrirLupa.onClick.AddListener(AbrirCerrarLupa);

        // Botones de zonas
        botonOjos.onClick.AddListener(() => MostrarZona(personajeActual?.lupaOjos));
        botonBoca.onClick.AddListener(() => MostrarZona(personajeActual?.lupaBoca));
        botonCuello.onClick.AddListener(() => MostrarZona(personajeActual?.lupaCuello));
        botonEspalda.onClick.AddListener(() => MostrarZona(personajeActual?.lupaEspalda));
    }

    void AbrirCerrarLupa()
    {
        panelLupa.SetActive(!panelLupa.activeSelf);

        if (panelLupa.activeSelf)
        {
            personajeActual = GameManager.instance.personajeActual;

            // Mostrar la imagen por defecto al abrir
            if (personajeActual != null && personajeActual.lupaDefault != null)
            {
                imagenZona.sprite = personajeActual.lupaDefault;
            }
        }
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


    public void DesactivarBotonLupa(){

        botonAbrirLupa.interactable = false;
    }

      public void ActivarBotonLupa(){
        
        botonAbrirLupa.interactable = false;
    }
}
