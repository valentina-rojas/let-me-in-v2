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
        // Clonamos la lista de prefabs para asegurarnos que no son objetos modificados
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

            // ⚠️ Asegurarse de que no haya otro personaje vivo
            if (personajeActualEnEscena != null)
            {
                Destroy(personajeActualEnEscena);
            }

            GameObject currentCharacter = Instantiate(candidate, spawnPoint.position, Quaternion.identity);
            personajeActualEnEscena = currentCharacter;

            interactionFinished = false;
            CharacterManager.instance.ResetearAtencion();

            CharacterAttributes atributos = currentCharacter.GetComponent<CharacterAttributes>();
            DialogueManager dialogueManager = currentCharacter.GetComponent<DialogueManager>();

            if (atributos != null)
            {
                GameManager.instance.EstablecerPersonajeActual(atributos);
            }
            else
            {
                Debug.LogError("El personaje instanciado no tiene CharacterAttributes.");
            }

            personajesRestantes--;
            GameManager.instance.uiManager.ActualizarContadorPersonas(personajesRestantes);

            yield return StartCoroutine(MoveCharacter(currentCharacter, destination.position));

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

        HabilitarDialogo();
    }

    public void EndInteraction()
    {
        if (!interactionFinished)
        {
            StartCoroutine(MostrarDialogoDeResultado());
        }
    }

    private IEnumerator MostrarDialogoDeResultado()
    {
        DialogueManager dialogueManager = FindObjectOfType<CharacterAttributes>()?.gameObject.GetComponent<DialogueManager>();

        if (dialogueManager != null)
        {
            //  dialogueManager.EmpezarDialogoResultado();
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

    private void HabilitarDialogo()
    {
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.EnableDialogue();
            Debug.Log("Dialogue habilitado.");

        }
        else
        {
            Debug.LogError("DialogueManager no encontrado al habilitar diálogo.");
        }
    }


    public IEnumerator MoverPersonaje(GameObject personaje, Vector3 destino)
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 startPosition = personaje.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            personaje.transform.position = Vector3.Lerp(startPosition, destino, elapsedTime / duration);
            yield return null;
        }

        personaje.transform.position = destino;
    }

    public void DetenerSpawn()
    {
        spawnActivo = false;
    }


    public GameObject GetCharacterActual()
    {
        return personajeActualEnEscena;
    }




}

