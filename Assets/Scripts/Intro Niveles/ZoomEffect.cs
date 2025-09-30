using UnityEngine;

public class ZoomEffect : MonoBehaviour
{
    [Header("Configuración del zoom")]
    public float zoomScale = 1.1f; // cuánto se agranda
    public float speed = 2f;       // velocidad de la animación

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        // Oscila entre el tamaño normal y el zoom
        float scale = Mathf.Lerp(1f, zoomScale, (Mathf.Sin(Time.time * speed) + 1f) / 2f);
        transform.localScale = initialScale * scale;
    }
}
