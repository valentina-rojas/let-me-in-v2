using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

     [Header("Diálogo")]
 [SerializeField] private GameObject dialoguePanelPersonaje;
  [SerializeField] private GameObject dialoguePanelGuardia;
    [SerializeField] private TMP_Text dialogueTextPersonaje;
     [SerializeField] private TMP_Text dialogueTextGuardia;
         [SerializeField] private Button botonSiguientePersonaje;
         [SerializeField] private Button botonSiguienteGuardia;
 
    public GameObject GetDialoguePanelPersonaje() => dialoguePanelPersonaje;
    public GameObject GetDialoguePanelGuardia() => dialoguePanelGuardia;
    public TMP_Text GetDialogueTextPersonaje() => dialogueTextPersonaje;
    public TMP_Text GetDialogueTextGuardia() => dialogueTextGuardia;
      public Button  GetBotonSiguientePersonaje () => botonSiguientePersonaje;
        public Button  GetBotonSiguienteGuardia () => botonSiguienteGuardia;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
