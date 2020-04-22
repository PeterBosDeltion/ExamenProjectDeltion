using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractable : MonoBehaviour
{

    private void Awake()
    {
            //InputManager.interactEvent += Interact;
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (Input.GetButton("Interact"))
                Interact();
        }
    }

    private void Interact()
    {
        TutorialManager.instance.Interacted();
    }
}
