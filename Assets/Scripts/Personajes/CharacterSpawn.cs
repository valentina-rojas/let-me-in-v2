using UnityEngine;
using System.Collections;

public class CharacterSpawn : MonoBehaviour
{
    private GameObject[] characters;

    public Transform spawnPoint;
    public Transform destination;
    public Transform exitPoint;

    private int currentIndex = 0;
    private bool interactionFinished = false;

    public bool spawnActivo = true;
    private GameObject personajeActualEnEscena;

    private int totalPersonajes;
    private int personajesRestantes;

    public void AsignarPersonajesDelNivel(GameObject[] personajesDelNivel)
    {
        characters = new GameObject[personajesDelNivel.Length];
        for (int i = 0; i < personajesDelNivel.Length; i++)
        {
            characters[i] = personajesDelNivel[i];
        }

        totalPersonajes = characters.Length;
        personajesRestantes = totalPersonajes;

        GameManager.instance.uiManager.ActualizarContadorPersonas(personajesRestantes);
    }

    public void ComenzarSpawn()
    {
        currentIndex = 0;
        StartCoroutine(SpawnCharacters());
    }

    IEnumerator SpawnCharacters()
    {
        while (currentIndex < characters.Length && spawnActivo)
        {
            GameObject candidate = characters[currentIndex];

            if (personajeActualEnEscena != null)
                Destroy(personajeActualEnEscena);

            GameObject currentCharacter = Instantiate(candidate, spawnPoint.position, Quaternion.identity);
            personajeActualEnEscena = currentCharacter;

            interactionFinished = false;
            CharacterManager.instance.ResetearAtencion();

            CharacterAttributes atributos = currentCharacter.GetComponent<CharacterAttributes>();

            if (atributos != null)
                GameManager.instance.EstablecerPersonajeActual(atributos);

            personajesRestantes--;
            GameManager.instance.uiManager.ActualizarContadorPersonas(personajesRestantes);

            // Mover personaje hacia destino
            yield return StartCoroutine(MoveCharacter(currentCharacter, destination.position));

            // Esperar a que termine la interacción
            yield return new WaitUntil(() => interactionFinished);

            Destroy(currentCharacter);
            personajeActualEnEscena = null;
            currentIndex++;

            yield return new WaitForSeconds(2f);
        }

        if (spawnActivo)
        {
            Debug.Log("Todos los personajes han pasado.");
            GameManager.instance.FinDeNivel();
        }
    }

    IEnumerator MoveCharacter(GameObject character, Vector3 targetPosition)
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 startPosition = character.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        character.transform.position = targetPosition;

        // Mostrar diálogo inicial
        HabilitarDialogoInicial();
    }

    public void EndInteraction()
    {
        if (!interactionFinished)
            StartCoroutine(MostrarDialogoDeResultado());
    }

    private IEnumerator MostrarDialogoDeResultado()
    {
        DialogueManager dialogueManager = DialogueManager.instance;

        if (dialogueManager != null)
        {
            yield return new WaitUntil(() => dialogueManager.HaTerminadoElDialogo());
        }
        else
        {
            Debug.LogError("DialogueManager no encontrado.");
        }

        interactionFinished = true;
    }

    public void FinalizarInteraccion()
    {
        interactionFinished = true;
    }

    // 🔹 Diálogo inicial
private void HabilitarDialogoInicial()
{
    if (personajeActualEnEscena == null) return;

    CharacterAttributes atributos = personajeActualEnEscena.GetComponent<CharacterAttributes>();
    if (atributos == null) return;

    // ⚠ Indicar que es diálogo inicial

    DialogueManager.instance.IniciarDialogoDePersonaje(atributos, atributos.nodoInicial, false);
}

    // 🔹 Diálogo de respuesta (ingreso o rechazo)
    public void HabilitarDialogoRespuesta(bool ingreso)
    {
       if (personajeActualEnEscena == null) return;

    CharacterAttributes atributos = personajeActualEnEscena.GetComponent<CharacterAttributes>();
    if (atributos == null) return;

    string nodo = ingreso ? atributos.respuestaIngreso : atributos.respuestaRechazo;

    DialogueManager.instance.IniciarDialogoDePersonaje(atributos, nodo, true);
    }

    public IEnumerator MoverPersonaje(GameObject personaje, Vector3 destino)
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 startPosition = personaje.transform.position;

        AudioManager.instance.sonidoPasosPersonaje.Play();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            personaje.transform.position = Vector3.Lerp(startPosition, destino, elapsedTime / duration);
            yield return null;
        }

        personaje.transform.position = destino;

        AudioManager.instance.sonidoPasosPersonaje.Stop();
    }

    public void DetenerSpawn()
    {
        spawnActivo = false;
    }

    public GameObject GetCharacterActual()
    {
        return personajeActualEnEscena;
    }

    public int GetCurrentIndex()
    {
        return currentIndex + 1; // Devuelve el índice del personaje actual
    }
}
