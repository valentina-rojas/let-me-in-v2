using UnityEngine;
using UnityEngine.UI;

public class StrikesBar : MonoBehaviour
{
    public Slider barraStrikes;   
    public float maxStrikes = 3f;     
    private float strikesActuales = 0f;

    public Image fillBarImageStrikes;  

    public Color colorVerde = Color.green;
    public Color colorAmarillo = Color.yellow;
    public Color colorRojo = Color.red;

    public AudioSource loadingBar;

    void Start()
    {
        if (barraStrikes != null)
        {
            barraStrikes.maxValue = maxStrikes;
            barraStrikes.value = strikesActuales;
        }
    }


    public void ActualizarBarraStrikes()
    {
      
        strikesActuales = GameManager.instance.strikes;

 
        if (strikesActuales > maxStrikes)
        {
            strikesActuales = maxStrikes;
        }


        if (barraStrikes != null)
        {
            barraStrikes.value = strikesActuales;
            ActualizarColorBarra();
            AudioManager.instance.sonidoLoading.Play();
        }


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
