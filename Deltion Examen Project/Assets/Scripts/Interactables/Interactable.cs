using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public float holdDuration;
    public Image holdCircle;
    public TextMeshProUGUI interactText;
    protected List<Player> nearbyPlayers = new List<Player>();

    protected bool interacted = false;

    private void Awake()
    {
        //InputManager.interactEvent += Interact;
        holdCircle.fillAmount = 0;
        if(holdDuration <= 0)
        {
            interactText.text = "Press " + "\"" + "E" + "\"";
        }
        else
        {
            interactText.text = "Hold " + "\"" + "E" + "\"";
        }

        interactText.gameObject.SetActive(false);
    }
    public void OnTriggerStay(Collider other)
    {
        if (canInteract)
        {
            if (other.GetComponent<Player>())
            {
                if(!interacted)
                    interactText.gameObject.SetActive(true);
                if ((other.GetComponent<PlayerController>().myInputManager.GetTimeInteractHeld() > 0))
                {
                    interactText.gameObject.SetActive(false);
                    holdCircle.enabled = true;

                }
                if (!interacted)
                {
                    holdCircle.fillAmount = (other.GetComponent<PlayerController>().myInputManager.GetTimeInteractHeld() / holdDuration);
                }
                else
                {
                    holdCircle.fillAmount = 0;
                }

                if ((other.GetComponent<PlayerController>().myInputManager.GetTimeInteractHeld() >= holdDuration && !interacted))
                {
                    Interact();
                }
            }
        }
      
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (!nearbyPlayers.Contains(other.GetComponent<Player>()))
            {
                nearbyPlayers.Add(other.GetComponent<Player>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (nearbyPlayers.Contains(other.GetComponent<Player>()))
            {
                nearbyPlayers.Remove(other.GetComponent<Player>());
            }
        }

        if (nearbyPlayers.Count <= 0)
        {
            interacted = false;
            interactText.gameObject.SetActive(false);
        }
    }


    protected abstract void Interact();
}
