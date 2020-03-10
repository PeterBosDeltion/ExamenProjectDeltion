using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool focusOnPlayer = true;
    public Transform target;
    [Tooltip("Use this offset to apply a distance between the player and the camera")]
    public float distanceOffset;
    [Tooltip("Use this offset to move the camera Up or Down incase the pivot of the player is to low or high making the follow look slightly off")]
    public float heightOfset;
    [Tooltip("The lower the value the slower/smoother the camera will follow the player")]
    public float smoothness = 5;

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
        Vector3 newOffset = Camera.main.transform.forward * -distanceOffset;
        newOffset += Camera.main.transform.up * heightOfset;
        Vector3 targetPos = target.position + newOffset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothness * Time.deltaTime);

        transform.localPosition = smoothedPos;
    }
}