using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public enum CharacterState { Sano, Enfermo }

    public string nombre;
    public CharacterState estado;


    [Header("Diálogos Ink")]
    public string nodoInicial = "introduccion";        
    public string respuestaIngreso = "respuestaIngreso";   
    public string respuestaRechazo = "respuestaRechazo";   
    public TextAsset inkJSON; // tu archivo Ink

    public GameObject prefab;
    public bool esAgresivo;

    [HideInInspector] public Animator animator;

   void Awake()
{
    animator = GetComponent<Animator>();
    if (animator == null)
        Debug.LogError($"Animator no encontrado en {gameObject.name}");

    if (string.IsNullOrEmpty(nodoInicial))
        nodoInicial = "introduccion";
    if (string.IsNullOrEmpty(respuestaIngreso))
        respuestaIngreso = "respuestaIngreso";
    if (string.IsNullOrEmpty(respuestaRechazo))
        respuestaRechazo = "respuestaRechazo";
}

    
}
