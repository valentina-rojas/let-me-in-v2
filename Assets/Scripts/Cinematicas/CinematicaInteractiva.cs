using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using static EventManager;


public class CinematicaInteractiva : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPopUpsMails;
    public GameObject panelMailExpandido;

    [SerializeField] private Button botonPopup;
    [SerializeField] private Button botonExpandirMail;
    [SerializeField] private Button botonResponder; 

    public TMP_Text respuestasMinisterio;

    private void Start()
    {
        botonPopup.onClick.AddListener(AbrirPopUps);
        botonExpandirMail.onClick.AddListener(ExpandirMails);
    }

    public void AbrirPopUps()
    {
        botonPopup.interactable = false;
        botonPopup.gameObject.SetActive(false);
        panelPopUpsMails.SetActive(true);

        // Detiene el efecto de zoom del botón
        ZoomEffect zoom = botonPopup.GetComponent<ZoomEffect>();
        if (zoom != null)
            zoom.enabled = false;
    }

    private void ExpandirMails()
    {
        panelPopUpsMails.SetActive(false);
        panelMailExpandido.SetActive(true);
    }

    public void ResponderMail()
    {
        

    }



}
