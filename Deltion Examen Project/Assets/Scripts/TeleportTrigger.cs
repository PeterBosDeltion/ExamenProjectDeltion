using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public Transform targetPosition;
    public GameObject toActivate;
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.transform.position = targetPosition.position;
            toActivate.SetActive(true);
        }
      
    }
}
