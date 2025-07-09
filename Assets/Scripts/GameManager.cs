using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private CharacterSpawn characterSpawn;
    private SpriteRenderer spriteRendererPersonaje;
    public CharacterAttributes personajeActual;
    public UIManager uiManager;
    public CharacterManager characterManager;

    public string[] mensajesInicioDia;

    public int sanosIngresados;
    public int enfermosIngresados;
    public int sanosRechazados;
    public int enfermosRechazados;
    public int strikes;

    public int NivelActual { get; private set; }

    private GameObject personajeGOActual;

    [System.Serializable]
    public class Nivel
    {
        public GameObject[] personajesDelNivel;
    }

    [Header("Niveles del juego")]
    public Nivel[] niveles;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        strikes = 0;

        NivelActual = GameData.NivelActual;
        uiManager = FindFirstObjectByType<UIManager>();
        characterSpawn = FindFirstObjectByType<CharacterSpawn>();

        if (uiManager == null)
            Debug.LogError("UIManager no encontrado en la escena.");
        if (characterSpawn == null)
            Debug.LogError("CharacterSpawn no encontrado en la escena.");

        // Mostrar mensaje inicio día y esperar a que termine antes de iniciar spawn
        if (NivelActual - 1 < mensajesInicioDia.Length)
        {
            uiManager.PanelInicioDesactivado += IniciarSpawnDePersonajes; // Suscribirse al evento
            uiManager.MostrarInicioDia(mensajesInicioDia[NivelActual - 1]);
        }
        else
        {
            Debug.LogWarning("No hay mensaje definido para este día.");
            // Si no hay mensaje, arrancar spawn directo
            IniciarSpawnDePersonajes();
        }
    }


    public void IniciarSpawnDePersonajes()
    {
        if (NivelActual - 1 < niveles.Length)
        {
            characterSpawn.AsignarPersonajesDelNivel(niveles[NivelActual - 1].personajesDelNivel);
            characterSpawn.ComenzarSpawn();
        }
        else
        {
            Debug.LogWarning("No hay más niveles definidos.");
        }

    }

    public void EstablecerPersonajeActual(CharacterAttributes personaje)
    {
        personajeActual = personaje;
        spriteRendererPersonaje = personajeActual.GetComponent<SpriteRenderer>();
        personajeGOActual = personaje.gameObject;
    }


    public void OnBotonIngresoClick()
    {
        StartCoroutine(ProcesoIngreso());
    }

    public void OnBotonRechazoClick()
    {
        StartCoroutine(ProcesoRechazo());
    }

    private IEnumerator ProcesoIngreso()
    {
        VerificarEstadoPersonaje(true);

        // personajeActual.animator.SetTrigger("reaccionIngreso");

        DialogueManager dialogueManager = personajeActual.GetComponent<DialogueManager>();
        dialogueManager.ComenzarDialogoRespuesta(personajeActual.respuestaIngreso);

        yield return new WaitUntil(() => dialogueManager.HaTerminadoElDialogo());

        yield return StartCoroutine(characterSpawn.MoverPersonaje(personajeGOActual, characterSpawn.exitPoint.position));


        characterSpawn.FinalizarInteraccion();
    }


    private IEnumerator ProcesoRechazo()
    {
        VerificarEstadoPersonaje(false);

        //  personajeActual.animator.SetTrigger("reaccionRechazo");

        DialogueManager dialogueManager = personajeActual.GetComponent<DialogueManager>();
        dialogueManager.ComenzarDialogoRespuesta(personajeActual.respuestaRechazo);

        yield return new WaitUntil(() => dialogueManager.HaTerminadoElDialogo());

        // Mover personaje de vuelta al punto de origen
        yield return StartCoroutine(characterSpawn.MoverPersonaje(personajeGOActual, characterSpawn.spawnPoint.position));

        characterSpawn.FinalizarInteraccion();
    }


    public void VerificarEstadoPersonaje(bool esIngreso)
    {
        if (personajeActual == null) return;

        CharacterAttributes.CharacterState estado = personajeActual.estado;

        if (esIngreso)
        {
            if (estado == CharacterAttributes.CharacterState.Sano)
            {
                Debug.Log("✅ Decisión correcta: personaje sano ingresado.");
                sanosIngresados++;
            }
            else // estado == Enfermo
            {
                Debug.Log("❌ Decisión incorrecta: personaje enfermo ingresado.");
                enfermosIngresados++;
                strikes++;
            }
        }
        else // es un rechazo
        {
            if (estado == CharacterAttributes.CharacterState.Sano)
            {
                Debug.Log("❌ Decisión incorrecta: personaje sano rechazado.");
                sanosRechazados++;
                strikes++;
            }
            else // estado == Enfermo
            {
                Debug.Log("✅ Decisión correcta: personaje enfermo rechazado.");
                enfermosRechazados++;
            }
        }

        // Podés poner lógica extra si querés que al llegar a cierto número de strikes termine el juego:
        /*
        if (strikes >= 3)
        {
            FinDeJuego();
        }
        */
    }


    public void MostrarMensaje()
    {

        if (enfermosIngresados == 0 && sanosRechazados == 0)
        {
            uiManager.mensajeReporte.text = "¡Buen trabajo!";

            if (NivelActual == 1)
            {
                uiManager.botonSiguienteNivel.gameObject.SetActive(true);
            }

            if (NivelActual == 2)
            {
                uiManager.botonGanaste.gameObject.SetActive(true);
            }
        }
        else
        {
            uiManager.mensajeReporte.text = "Más cuidado la próxima vez...";

            if (NivelActual == 1)
            {
                uiManager.botonSiguienteNivel.gameObject.SetActive(true);
            }

            if (NivelActual == 2)
            {
                uiManager.botonGanaste.gameObject.SetActive(true);
            }
        }

    }

    public void FinDeNivel()
    {

  // Llamar a UIManager para mostrar el reporte con los datos actuales
    if (uiManager != null)
    {
        uiManager.ActualizarPanelReporte(sanosIngresados, enfermosIngresados, sanosRechazados, enfermosRechazados);
    }
    else
    {
        Debug.LogError("UIManager no asignado en GameManager.");
    }
    }

}
