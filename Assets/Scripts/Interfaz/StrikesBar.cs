using UnityEngine;
using UnityEngine.UI;

public class StrikesBar : MonoBehaviour
{
    public Slider barraStrikes;    // La barra que representa el rechazo de sanos
    public float maxStrikes = 3f;      // Máximo número de rechazos permitidos
    private float strikesActuales = 0f; // Rechazos actuales

    public Image fillBarImageStrikes;   // Imagen de la barra de relleno

    public Color colorVerde = Color.green;
    public Color colorAmarillo = Color.yellow;
    public Color colorRojo = Color.red;

    public AudioSource loadingBar;

    void Start()
    {
        // Inicializar la barra con 0 rechazos
        if (barraStrikes != null)
        {
            barraStrikes.maxValue = maxStrikes;
            barraStrikes.value = strikesActuales;
        }
    }

    // Método para actualizar la barra cuando rechazas a un personaje sano
    public void ActualizarBarraStrikes()
    {
        // Obtener el valor de strikes del GameManager
        strikesActuales = GameManager.instance.strikes;

        // Asegurarse de no exceder el nivel máximo 
        if (strikesActuales > maxStrikes)
        {
            strikesActuales = maxStrikes;
        }

        // Actualizar la barra visualmente
        if (barraStrikes != null)
        {
            barraStrikes.value = strikesActuales;
            ActualizarColorBarra();
            // loadingBar.Play();
        }

        // Verificar condición de derrota
        if (strikesActuales >= maxStrikes)
        {
            Debug.Log("¡Has alcanzado el máximo de strikes! Has perdido.");
            PerderJuego();
        }
    }


    private void ActualizarColorBarra()
    {
        float porcentajeRechazo = strikesActuales / maxStrikes;

        if (porcentajeRechazo <= 0.34f)
        {
            fillBarImageStrikes.color = colorVerde;
        }
        else if (porcentajeRechazo <= 0.67f)
        {
            fillBarImageStrikes.color = colorAmarillo;
        }
        else
        {
            fillBarImageStrikes.color = colorRojo;
        }
    }


    private void PerderJuego()
    {
        GameManager.instance.GameOver(GameManager.TipoDerrota.Despido);
    }


}
