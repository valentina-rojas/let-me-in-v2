using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterSpawn characterSpawn;
    public CharacterAttributes personajeActual;
    public UIManager uiManager;
    public CharacterManager characterManager;
    public StressBar stressBar;
    public StrikesBar strikesBar;
    public DoorController doorController;
    public RadioManager radioManager;
    public CheckCondition checkCondition;
    public CharacterSelector characterSelector;
    public ReproducirCinematicas reproducirCinematicas;

    private SpriteRenderer spriteRendererPersonaje;


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


    public enum TipoDerrota
    {
        Estres,
        Despido,
        Disturbios
    }


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
        if (NivelActual - 1 >= niveles.Length)
        {
            Debug.LogWarning("No hay más niveles definidos.");
            return;
        }

        GameObject[] todosLosPersonajes = niveles[NivelActual - 1].personajesDelNivel;

        if (todosLosPersonajes.Length < 6)
        {
            Debug.LogWarning("⚠ No hay suficientes personajes para seleccionar 6. Usando todos.");
            characterSpawn.AsignarPersonajesDelNivel(todosLosPersonajes);
            characterSpawn.ComenzarSpawn();
            return;
        }

        GameObject[] seleccionados = characterSelector.SeleccionarPersonajesConAgresivos(todosLosPersonajes, 6, 2);
        characterSpawn.AsignarPersonajesDelNivel(seleccionados);
        characterSpawn.ComenzarSpawn();
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
        checkCondition.DesactivarBotonMedico();

        VerificarEstadoPersonaje(true);
        personajeActual.animator.SetTrigger("reaccionIngreso");

        DialogueManager dialogueManager = personajeActual.GetComponent<DialogueManager>();
        dialogueManager.ComenzarDialogoRespuesta(personajeActual.respuestaIngreso);

        yield return new WaitUntil(() => dialogueManager.HaTerminadoElDialogo());

        // Abrir puerta
        yield return StartCoroutine(doorController.Abrir());

        // Mover personaje adentro mientras la puerta está abierta
        yield return StartCoroutine(characterSpawn.MoverPersonaje(personajeGOActual, characterSpawn.exitPoint.position));

        // Cerrar puerta después de que el personaje terminó de entrar
        yield return StartCoroutine(doorController.Cerrar());

        if (personajeActual.estado == CharacterAttributes.CharacterState.Enfermo)
        {
            radioManager.ActivarDisturbios();
        }
        else
        {
            characterSpawn.FinalizarInteraccion();
        }

        stressBar.ActualizarEstres(1f);
    }


    private IEnumerator ProcesoRechazo()
    {
        checkCondition.DesactivarBotonMedico();

        VerificarEstadoPersonaje(false);

        personajeActual.animator.SetTrigger("reaccionRechazo");

        DialogueManager dialogueManager = personajeActual.GetComponent<DialogueManager>();
        dialogueManager.ComenzarDialogoRespuesta(personajeActual.respuestaRechazo);

        yield return new WaitUntil(() => dialogueManager.HaTerminadoElDialogo());

        // Mover personaje de vuelta al punto de origen
        yield return StartCoroutine(characterSpawn.MoverPersonaje(personajeGOActual, characterSpawn.spawnPoint.position));

        characterSpawn.FinalizarInteraccion();
        stressBar.ActualizarEstres(1f);
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
                strikesBar.ActualizarBarraStrikes();
            }
        }
        else // es un rechazo
        {
            if (estado == CharacterAttributes.CharacterState.Sano)
            {
                Debug.Log("❌ Decisión incorrecta: personaje sano rechazado.");
                sanosRechazados++;
                strikes++;
                strikesBar.ActualizarBarraStrikes();
            }
            else // estado == Enfermo
            {
                Debug.Log("✅ Decisión correcta: personaje enfermo rechazado.");
                enfermosRechazados++;
            }
        }
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


    public void GameOver(TipoDerrota tipo)
    {
        characterSpawn?.DetenerSpawn();

        if (personajeGOActual != null)
        {
            Destroy(personajeGOActual);
            personajeGOActual = null;
        }

        uiManager?.MostrarPantallaDerrota(tipo);
    }

    public void SiguienteNivel()
    {
        Debug.Log("Botón Siguiente Nivel presionado");

        NivelActual++;
        GameData.NivelActual = NivelActual;
        //  GameData.Faltas = strikesAcumulados;
        //  GameData.DialogosOmitidos = dialogosOmitidosTotal;

        if (NivelActual > niveles.Length)
        {
            Debug.Log("¡No hay más niveles! Fin del juego.");

            reproducirCinematicas.CinematicaGanar();
            return;
        }

        // Si hay niveles, recargamos la escena para cargar el nuevo nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
