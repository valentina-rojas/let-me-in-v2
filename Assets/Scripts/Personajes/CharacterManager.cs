using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    
    private bool yaAtendido = false;

    public CharacterAttributes UltimoPersonajeAtendido { get; private set; }

    private List<CharacterAttributes> personajesAtendidos = new List<CharacterAttributes>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AtenderPersonaje(CharacterAttributes personaje)
    {
        if (yaAtendido) return;

        UltimoPersonajeAtendido = personaje;

        if (!personajesAtendidos.Contains(personaje))
        {
            personajesAtendidos.Add(personaje);
        }

        yaAtendido = true;
    }

    public void ResetearAtencion()
    {
        yaAtendido = false;
        UltimoPersonajeAtendido = null;
    }

    public List<CharacterAttributes> GetPersonajesAtendidos()
    {
        return personajesAtendidos;
    }

    public void ResetearHistorial()
    {
        personajesAtendidos.Clear();
    }



}