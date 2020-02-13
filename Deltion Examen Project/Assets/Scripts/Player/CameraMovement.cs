using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool focusOnPlayer = true;
    Transform target;
    [Tooltip("Use this offset to apply a distance between the player and the camera (this has the same effect as applying the same value in all axises of the position offset)")]
    public float distanceOffset;
    [Tooltip("The lower the value the slower/smoother the camera will follow the player")]
    public float Smoothness = 5;

    private void Awake()
    {
        target = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    //Either late or fixed update CAN work with this.
    private void FixedUpdate()
    {
        if (focusOnPlayer)
        {
            MoveAfterPlayer();
        }
    }

    //Camera follows player
    void MoveAfterPlayer()
    {
        Vector3 newOffset = new Vector3(distanceOffset, distanceOffset, 0);
        Vector3 targetPos = target.position + newOffset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, Smoothness * Time.deltaTime);

        transform.position = smoothedPos;
    }
}