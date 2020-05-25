using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePosition : MonoBehaviour
{
    public Player myPlayer;

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "DropPod" && myPlayer)
        {
            myPlayer.transform.position = transform.position;
            myPlayer.gameObject.SetActive(true);
            myPlayer.reviveEvent.Invoke();

            Destroy(other.gameObject);
            FindObjectOfType<CameraMovement>().RemoveDeadPlayer();
            myPlayer = null;

        }
    }
}
