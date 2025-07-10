using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AggressiveNPCs : MonoBehaviour
{
    public static AggressiveNPCs instance;


    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject PanelSeguridad;
    public GameObject PanelTimer;
    public Button botonSeguridad;

    [Header("Audio")]
    public AudioSource audioSeguridad;
    public AudioSource pasosSeguridad;
    public AudioSource escobaSeguridad;
    public AudioSource golpe;
    public AudioSource sonidoBoton;

    [Header("Managers & References")]
    public CharacterSpawn characterSpawn;
    public StressBar stressBar;
    public DoorController doorController;

    public Transform spawnPointSeguridad;
    public Transform targetPoint;
    public Transform cameraTransform;

    [Header("Vidrios Rotos")]
    public GameObject[] vidriosRotos;

    [Header("Shake Settings")]
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 3f;

    [Header("Temporizador")]
    public float tiempoBaseTemporizador = 5f;

    [Header("Velocidad de Movimiento")]
    public float velocidadPersecucion = 5f;
    public float velocidadGuardia = 7f;
    public float distanciaSeguridadYAgresivo = 0.1f;



    private Coroutine toggleCoroutine;
    private Coroutine shakeCoroutine;

    private GameObject seguridadInstance;
    private Vector3 originalCameraPosition;
    private bool temporizadorActivo = false;
    private float tiempoRestante;
    private int vidrioActual = -1;
    private bool isShaking = false;
    private float tiempoInicioAgresion;

    public GameObject seguridadPrefab;

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
    void Start()
    {
        InitializeState();
    }

    void Update()
    {
        if (temporizadorActivo)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                temporizadorActivo = false;
                ActualizarTextoTemporizador();
                FinTemporizador();
            }
            else
            {
                ActualizarTextoTemporizador();
            }
        }
    }

    #region Inicialización y UI
    void InitializeState()
    {
        audioSeguridad.Stop();
        pasosSeguridad.Stop();
        escobaSeguridad.Stop();
        botonSeguridad.interactable = false;

        if (cameraTransform != null)
            originalCameraPosition = cameraTransform.position;

        // Ajustar tiempo base según nivel
        tiempoBaseTemporizador = (GameManager.instance.NivelActual == 2) ? 3f : 5f;

        timerText.gameObject.SetActive(false);
        PanelTimer.SetActive(false);
        PanelSeguridad.SetActive(false);
    }

    void ActualizarTextoTemporizador()
    {
        if (timerText == null) return;
        timerText.text = Mathf.CeilToInt(tiempoRestante).ToString();
    }

    void StartTimer(float tiempo)
    {
        tiempoRestante = tiempo;
        temporizadorActivo = true;
        timerText.gameObject.SetActive(true);
        PanelTimer.SetActive(true);
        ActualizarTextoTemporizador();
    }
    #endregion

    #region Comportamiento agresivo
    public void MostrarComportamientoAgresivo()
    {
        Debug.Log("¡El personaje está actuando de manera agresiva!");

        tiempoInicioAgresion = Time.time;

        if (temporizadorActivo)
            temporizadorActivo = false;

        botonSeguridad.interactable = true;
        StartTimer(tiempoBaseTemporizador);
        ActivarPeligro();

        if (cameraTransform != null)
            StartCameraShake();

        ActivarVidrioRotoSiguiente();
    }

    void ActivarVidrioRotoSiguiente()
    {
        if (vidriosRotos == null || vidriosRotos.Length == 0) return;

        // Desactivar vidrio anterior si corresponde
        if (vidrioActual >= 0 && vidrioActual < vidriosRotos.Length)
            vidriosRotos[vidrioActual].SetActive(false);

        vidrioActual = (vidrioActual + 1) % vidriosRotos.Length;
        vidriosRotos[vidrioActual]?.SetActive(true);
    }
    #endregion

    #region Cámara y animaciones
    void StartCameraShake()
    {
        if (!isShaking)
            shakeCoroutine = StartCoroutine(ShakeCameraCoroutine());
    }

    IEnumerator ShakeCameraCoroutine()
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            cameraTransform.position = originalCameraPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = originalCameraPosition;
        isShaking = false;
    }
    #endregion

    #region Panel y audio peligro
    void ActivarPeligro()
    {
        if (!PanelSeguridad.activeInHierarchy && toggleCoroutine == null)
        {
            toggleCoroutine = StartCoroutine(TogglePanelSeguridad());
            ReproducirAnimacionPersonajePeligro(true);
        }
    }

    IEnumerator TogglePanelSeguridad()
    {
        while (true)
        {
            PanelSeguridad.SetActive(true);
            audioSeguridad.Play();

            yield return new WaitForSeconds(0.5f);

            PanelSeguridad.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }
    }

    void ReproducirAnimacionPersonajePeligro(bool activar)
    {
        var personaje = characterSpawn.GetCharacterActual();
        if (personaje == null) return;

        var animController = personaje.GetComponent<NPCAnimationController>();
        if (activar)
            animController?.StartDangerAnimation();
        else
            animController?.StopDangerAnimation();
    }

    public void DetenerPeligro()
    {

        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            toggleCoroutine = null;
            PanelSeguridad.SetActive(false);
            PanelTimer.SetActive(false);
            audioSeguridad.Stop();
        }

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            if (cameraTransform != null)
                cameraTransform.position = originalCameraPosition;
            isShaking = false;
        }

        ReproducirAnimacionPersonajePeligro(false);
    }
    #endregion

    #region Eventos 
    void FinTemporizador()
    {
        DetenerPeligro();

        PerderJuego();
    }
    #endregion

    #region Seguridad y empujar agresivo
    public void LlamarSeguridad()
    {
        sonidoBoton.Play();
        DetenerPeligro();
        botonSeguridad.interactable = false;

        if (temporizadorActivo)
        {
            temporizadorActivo = false;
            timerText.gameObject.SetActive(false);
        }

        StartCoroutine(EsperarAntesDeLlamarSeguridad(3f));
    }

    IEnumerator EsperarAntesDeLlamarSeguridad(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Llamar al método Abrir de la instancia DoorController
        if (GameManager.instance != null && GameManager.instance.doorController != null)
        {
            yield return StartCoroutine(GameManager.instance.doorController.Abrir());
        }
        else
        {
            Debug.LogError("DoorController no asignado en GameManager.instance");
        }

        seguridadInstance = Instantiate(seguridadPrefab, spawnPointSeguridad.position, Quaternion.identity);

        pasosSeguridad.Play();
        escobaSeguridad.Play();

        var personaje = characterSpawn.GetCharacterActual();
        if (personaje != null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(EmpujarPersonajeAgresivo(personaje.transform));
        }
    }

    IEnumerator EmpujarPersonajeAgresivo(Transform personaje)
    {
        Vector3 puntoFueraPantalla = new Vector3(targetPoint.position.x - 1f, personaje.position.y, personaje.position.z);

        while (Vector3.Distance(personaje.position, puntoFueraPantalla) > 0.1f)
        {
            personaje.position = Vector3.MoveTowards(personaje.position, puntoFueraPantalla, velocidadPersecucion * Time.deltaTime);

            if (seguridadInstance != null)
            {
                Vector3 posSeguridad = personaje.position - new Vector3(distanciaSeguridadYAgresivo, 0, 0);
                seguridadInstance.transform.position = Vector3.MoveTowards(seguridadInstance.transform.position, posSeguridad, velocidadGuardia * Time.deltaTime);
            }
            yield return null;
        }

        if (personaje != null)
            Destroy(personaje.gameObject);

        golpe.Play();
        stressBar.ActualizarEstres(1);

        // Voltear seguridad para regresar
        var escalaOriginal = seguridadInstance.transform.localScale;
        if (escalaOriginal.x > 0)
            seguridadInstance.transform.localScale = new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);

        while (Vector3.Distance(seguridadInstance.transform.position, spawnPointSeguridad.position) > 0.1f)
        {
            seguridadInstance.transform.position = Vector3.MoveTowards(seguridadInstance.transform.position, spawnPointSeguridad.position, velocidadGuardia * Time.deltaTime);
            yield return null;
        }

        // 🔒 Cerrar la puerta después de que el guardia vuelve
        if (GameManager.instance != null && GameManager.instance.doorController != null)
        {
            yield return StartCoroutine(GameManager.instance.doorController.Cerrar());
        }
        else
        {
            Debug.LogError("DoorController no asignado en GameManager.instance");
        }


        if (seguridadInstance != null)
            Destroy(seguridadInstance);

        pasosSeguridad.Stop();
        escobaSeguridad.Stop();

        yield return new WaitForSeconds(1f);



        characterSpawn.FinalizarInteraccion();

    }

    #endregion



    private void PerderJuego()
    {
        GameManager.instance.GameOver(GameManager.TipoDerrota.Disturbios);
    }

}
