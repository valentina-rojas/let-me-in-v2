using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics; 
using static EventManager;

public class CheckCondition : MonoBehaviour
{
    [Header("Prefabs y Transforms")]
    public GameObject medicoPrefab;
    public GameObject cartelSanoPrefab;
    public GameObject cartelEnfermoPrefab;
    public GameObject luzEscanerPrefab;
    public Transform spawnPointMedico;
    public Transform centroPantalla;
    public Transform puntoSalidaMedico;
    public Transform spawnPointIngreso;
    public Transform spawnPointRechazo;
    public Transform spawnPointLuzEscaner;

    [Header("Referencias")]
    public DialogueManager dialogueManager;
    public LeverController leverController;
    public CharacterManager charactersManager;
    public CharacterSpawn characterSpawn;

    [Header("Botón")]
    public Button botonMedico;

    private GameObject medicoInstance;
    private bool enEvaluacion = false;

    private void Start()
    {
        // El botón comienza desactivado siempre
        botonMedico.interactable = false;
    }

    public void EvaluarSalud()
    {

        //  MedicalScanUsed
        RegisterScanUsedEvent();

        // Ya fue usado este turno o ya está en proceso
        if (enEvaluacion || dialogueManager.medicoUsado) return;


        AudioManager.instance.sonidoBotonPresionado.Play();

        if (medicoInstance == null)
        {
            enEvaluacion = true;

            medicoInstance = Instantiate(medicoPrefab, spawnPointMedico.position, Quaternion.identity);
            StartCoroutine(MedicoEvaluacionRoutine());

            botonMedico.interactable = false;
            dialogueManager.medicoUsado = true;  // Marcar como usado por este nivel
        }
    }

    private void RegisterScanUsedEvent()
    {
        // Debug para verificar
       
        Debug.Log($"ScanUsed - Nivel: {GameData.NivelActual}, Índice personaje: {characterSpawn.GetCurrentIndex()}");
        
        // Crear y configurar el evento
        ScanUsedEvent scanUsed = new ScanUsedEvent();
        scanUsed.level = GameData.NivelActual;
        scanUsed.charIndex = characterSpawn.GetCurrentIndex();
        
        // Grabar el evento 
        #if !UNITY_EDITOR
            AnalyticsService.Instance.RecordEvent(scanUsed);
        #else
            Debug.Log("[ANALYTICS] Evento ScanUsedEvent registrado");
        #endif
   
    }

    private IEnumerator MedicoEvaluacionRoutine()
    {
        leverController.DesactivarPalanca();

        AudioManager.instance.sonidoMovimientoEscaner.Play();

        yield return StartCoroutine(MoverPersonaje(medicoInstance.transform, centroPantalla.position));

        AudioManager.instance.sonidoMovimientoEscaner.Stop();
    
        yield return new WaitForSeconds(2f);

        EvaluarEstadoPersonaje();

        yield return new WaitForSeconds(7f);

        AudioManager.instance.sonidoMovimientoEscaner.Play();
        yield return StartCoroutine(MoverPersonaje(medicoInstance.transform, puntoSalidaMedico.position));
        AudioManager.instance.sonidoMovimientoEscaner.Stop();

        Destroy(medicoInstance);
        medicoInstance = null;

        leverController.ActivarPalanca();
        enEvaluacion = false;
    }

    private IEnumerator MoverPersonaje(Transform personaje, Vector3 destino)
    {
        float velocidadMovimiento = 3f;

        while (Vector3.Distance(personaje.position, destino) > 0.1f)
        {
            personaje.position = Vector3.MoveTowards(personaje.position, destino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }
    }

    private void EvaluarEstadoPersonaje()
    {
        GameObject personajeGO = GameManager.instance.characterSpawn.GetCharacterActual();
        CharacterAttributes personajeActual = personajeGO?.GetComponent<CharacterAttributes>();
            StartCoroutine(MostrarLuzEscanerYCartel(personajeActual));
        
    }

    private IEnumerator MostrarLuzEscanerYCartel(CharacterAttributes personajeActual)
    {
        GameObject luzEscanerInstance = Instantiate(luzEscanerPrefab, spawnPointLuzEscaner.position, Quaternion.identity);
        
        AudioManager.instance.sonidoLuzEscaner.Play();

        yield return new WaitForSeconds(3f);
        
        AudioManager.instance.sonidoLuzEscaner.Stop();

        Destroy(luzEscanerInstance);

        yield return new WaitForSeconds(2f);

        if (personajeActual.estado == CharacterAttributes.CharacterState.Sano)
        {
            Debug.Log("El personaje está sano.");
            StartCoroutine(MostrarCartel(cartelSanoPrefab, spawnPointIngreso));
        }
        else if (personajeActual.estado == CharacterAttributes.CharacterState.Enfermo)
        {
            Debug.Log("El personaje está enfermo.");
            StartCoroutine(MostrarCartel(cartelEnfermoPrefab, spawnPointRechazo));
        }
    }

    private IEnumerator MostrarCartel(GameObject cartelPrefab, Transform spawnPoint)
    {
        GameObject cartelInstance = Instantiate(cartelPrefab, spawnPoint.position, Quaternion.identity);

        AudioManager.instance.sonidoBeepEscaner.Play();

        yield return new WaitForSeconds(2f);
        Destroy(cartelInstance);
    }

    public void ReiniciarNivel()
    {
        dialogueManager.medicoUsado = false;
        enEvaluacion = false;
        botonMedico.interactable = false;
    }

    public void DesactivarBotonMedico()
    {
        botonMedico.interactable = false;
    }
}
