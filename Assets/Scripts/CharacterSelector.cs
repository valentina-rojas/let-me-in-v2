using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // Selección general aleatoria sin condiciones
    public GameObject[] SeleccionarPersonajesAleatorios(GameObject[] personajes, int cantidad)
    {
        GameObject[] copia = (GameObject[])personajes.Clone();

        for (int i = 0; i < copia.Length; i++)
        {
            int randomIndex = Random.Range(i, copia.Length);
            GameObject temp = copia[i];
            copia[i] = copia[randomIndex];
            copia[randomIndex] = temp;
        }

        GameObject[] seleccionados = new GameObject[cantidad];
        for (int i = 0; i < cantidad; i++)
        {
            seleccionados[i] = copia[i];
        }

        return seleccionados;
    }

    // Seleccionar con mínimo agresivos garantizados
    public GameObject[] SeleccionarPersonajesConAgresivos(GameObject[] todos, int total, int minAgresivos)
    {
        List<GameObject> agresivos = new List<GameObject>();
        List<GameObject> noAgresivos = new List<GameObject>();

        foreach (GameObject go in todos)
        {
            CharacterAttributes attr = go.GetComponent<CharacterAttributes>();
            if (attr != null && attr.esAgresivo)
                agresivos.Add(go);
            else
                noAgresivos.Add(go);
        }

        if (agresivos.Count < minAgresivos)
        {
            Debug.LogWarning($"⚠ No hay suficientes agresivos para mínimo {minAgresivos}, seleccionando aleatorio sin condición.");
            return SeleccionarPersonajesAleatorios(todos, total);
        }

        GameObject[] seleccionados = new GameObject[total];

        // Seleccionar agresivos
        for (int i = 0; i < minAgresivos; i++)
        {
            int idx = Random.Range(0, agresivos.Count);
            seleccionados[i] = agresivos[idx];
            agresivos.RemoveAt(idx);
        }

        // Completar con no agresivos y agresivos restantes
        List<GameObject> combinados = new List<GameObject>(noAgresivos);
        combinados.AddRange(agresivos);

        for (int i = minAgresivos; i < total; i++)
        {
            int idx = Random.Range(0, combinados.Count);
            seleccionados[i] = combinados[idx];
            combinados.RemoveAt(idx);
        }

        // Mezclar para no tener siempre agresivos al principio
        for (int i = 0; i < seleccionados.Length; i++)
        {
            int j = Random.Range(i, seleccionados.Length);
            (seleccionados[i], seleccionados[j]) = (seleccionados[j], seleccionados[i]);
        }

        return seleccionados;
    }
}
