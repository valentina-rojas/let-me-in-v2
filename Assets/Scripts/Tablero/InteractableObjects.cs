using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableObjects : MonoBehaviour
{
    [SerializeField]
    public List<EventTrigger> eventTriggers; // Asigna los EventTriggers en el Inspector


    public Animator papelesAnimator;
    public Animator pantallaAnimator;

    public AudioSource sonidoPapel;
    public AudioSource sonidoEstatica;

    public void DesactivarEventTriggers()
    {
        foreach (var trigger in eventTriggers)
        {
            if (trigger != null)
            {
                trigger.enabled = false;
            }
        }
    }

    public void ActivarEventTriggers()
    {
        foreach (var trigger in eventTriggers)
        {
            if (trigger != null)
            {
                trigger.enabled = true;
            }
        }
    }


    public void AnimarPapeles()
    {

        StartCoroutine(ActivarPapeles());
    }


  public IEnumerator ActivarPapeles()
    {
        papelesAnimator.SetTrigger("papelesFlip");
        //    sonidoPapel.Play();
        AudioManager.instance.sonidoPapelesMoviendose.Play();
        yield return new WaitForSeconds(2f);
        // sonidoPapel.Stop();
        AudioManager.instance.sonidoPapelesMoviendose.Stop();
        papelesAnimator.ResetTrigger("papelesFlip");  // Resetea el trigger
        papelesAnimator.SetTrigger("papelesIdle");
    }


    public void AnimarPantalla()
    {
        StartCoroutine(ActivarPantalla());
    }

   public IEnumerator ActivarPantalla()
    {
        pantallaAnimator.SetTrigger("pantallaMove");
    // sonidoEstatica.Play();
    AudioManager.instance.sonidoEstaticaRadio.Play();
        yield return new WaitForSeconds(2f);
        //sonidoEstatica.Stop();
        AudioManager.instance.sonidoEstaticaRadio.Stop();
        pantallaAnimator.ResetTrigger("pantallaMove");  // Resetea el trigger
        pantallaAnimator.SetTrigger("pantallaIdle");
    }


}
