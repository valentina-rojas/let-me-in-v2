using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public enum CharacterState { Sano, Enfermo }

    public string nombre;
    public CharacterState estado;

    [Header("Ink")]
    public TextAsset inkJSON; // Archivo exportado a JSON desde Ink

    [Header("Diálogos Ink")]
    public string respuestaIngreso = "respuestaIngreso";  // Nodo Ink que se activa si lo aceptás
    public string respuestaRechazo = "respuestaRechazo";  // Nodo Ink que se activa si lo rechazás

    public GameObject prefab;
    public bool esAgresivo;

    [HideInInspector] public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"Animator no encontrado en {gameObject.name}");
        }
    }
}
