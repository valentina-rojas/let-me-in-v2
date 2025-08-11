using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeverController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static LeverController instance;

    public Image palancaSprite;
    public Sprite palancaNeutral;
    public Sprite palancaDerecha;
    public Sprite palancaIzquierda;

    private Vector2 startPointerPosition;
    private bool isDragged = false;
    private bool decisionMade = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Se detectó más de un LeverController en la escena. Este será destruido.");
            Destroy(gameObject);
        }
    }


    public void Start()
    {
        DesactivarPalanca();
    }


    public void DesactivarPalanca()
    {
        palancaSprite.raycastTarget = false;
    }

    public void ActivarPalanca()
    {
        if (palancaSprite == null)
        {
            Debug.LogError("❌ palancaSprite no asignado en el Inspector.");
            return;
        }

        palancaSprite.raycastTarget = true;
        Debug.Log("✅ Palanca activada (raycastTarget = true)");
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        startPointerPosition = eventData.position;
        isDragged = true;
        decisionMade = false; // Resetea la decisión al empezar a arrastrar
    }

    public void OnDrag(PointerEventData eventData)
    {
        AudioManager.instance.sonidoPalanca.Play();
        if (isDragged)
        {
            Vector2 currentPointerPosition = eventData.position;
            float dragDistance = currentPointerPosition.x - startPointerPosition.x;

            if (dragDistance > 0) // Arrastrando hacia la derecha
            {
                palancaSprite.sprite = palancaDerecha;

                // Verificar si no se ha tomado una decisión aún
                if (!decisionMade)
                {
                    GameManager.instance.OnBotonIngresoClick();
                    decisionMade = true; // Marca que se ha tomado una decisión
                }
            }
            else if (dragDistance < 0) // Arrastrando hacia la izquierda
            {
                palancaSprite.sprite = palancaIzquierda;

                // Verificar si no se ha tomado una decisión aún
                if (!decisionMade)
                {
                    GameManager.instance.OnBotonRechazoClick();
                    decisionMade = true; // Marca que se ha tomado una decisión
                }
            }

            DesactivarPalanca();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragged = false;
        palancaSprite.sprite = palancaNeutral; // Regresar la palanca a su sprite neutral
        decisionMade = false; // Resetea la decisión al soltar la palanca
    }
}
