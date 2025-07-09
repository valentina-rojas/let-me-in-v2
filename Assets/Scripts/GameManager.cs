using UnityEngine;

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

     IniciarSpawnDePersonajes();

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
    }

     public void FinDeNivel()
    {
    }


    
}
