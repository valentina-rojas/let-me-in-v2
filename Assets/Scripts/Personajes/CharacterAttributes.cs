using UnityEngine;
using System.Collections.Generic;

public class CharacterAttributes : MonoBehaviour
{
    public enum CharacterState
    {
        Sano,
        Enfermo
    }

    public string nombre;
    public CharacterState estado;

    public List<string> dialogosPersonaje;
    public List<string> dialogosGuardia;

    public string[] respuestaIngreso;
    public string[] respuestaRechazo;

    public GameObject prefab;
    public bool esAgresivo;

    [HideInInspector] public Animator animator;
    
        void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError($"Animator no encontrado en {gameObject.name}");
        }
    }

}
