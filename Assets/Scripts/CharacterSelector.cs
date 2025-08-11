using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NivelConfiguracion
{
    public int nivel;
    public int cantidadTotalPersonajes;
    public int cantidadAgresivos;
}

public class CharacterSelector : MonoBehaviour
{
    [Header("Configuraciones por nivel")]
    public List<NivelConfiguracion> configuracionesPorNivel;

    // Obtiene configuración por nivel
    public NivelConfiguracion ObtenerConfiguracionNivel(int nivel) =>
        configuracionesPorNivel.Find(c => c.nivel == nivel);

    // Método genérico para mezclar lista (Fisher-Yates)
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    // Selecciona personajes según nivel y configuración
    public GameObject[] SeleccionarPersonajesPorNivel(GameObject[] todos, int nivel)
    {
        var config = ObtenerConfiguracionNivel(nivel);
        if (config == null)
        {
            Debug.LogWarning($"No se encontró configuración para el nivel {nivel}");
            return new GameObject[0];
        }

        // Filtrar agresivos y no agresivos
        var agresivos = new List<GameObject>();
        var noAgresivos = new List<GameObject>();

        foreach (var go in todos)
        {
            var attr = go.GetComponent<CharacterAttributes>();
            if (attr != null && attr.esAgresivo)
                agresivos.Add(go);
            else
                noAgresivos.Add(go);
        }

        // Verificar disponibilidad
        if (agresivos.Count < config.cantidadAgresivos || noAgresivos.Count < config.cantidadTotalPersonajes - config.cantidadAgresivos)
        {
            Debug.LogWarning("No hay suficientes personajes para la configuración, seleccionando aleatorio simple");
            return SeleccionarPersonajesAleatorios(todos, config.cantidadTotalPersonajes);
        }

        // Mezclar listas
        Shuffle(agresivos);
        Shuffle(noAgresivos);

        // Seleccionar los agresivos exactos y completar con no agresivos
        var seleccionados = new List<GameObject>();
        seleccionados.AddRange(agresivos.GetRange(0, config.cantidadAgresivos));
        seleccionados.AddRange(noAgresivos.GetRange(0, config.cantidadTotalPersonajes - config.cantidadAgresivos));

        // Mezclar selección final
        Shuffle(seleccionados);

        return seleccionados.ToArray();
    }

    // Selección aleatoria simple
    public GameObject[] SeleccionarPersonajesAleatorios(GameObject[] personajes, int cantidad)
    {
        var lista = new List<GameObject>(personajes);
        Shuffle(lista);
        return lista.GetRange(0, cantidad).ToArray();
    }
}
