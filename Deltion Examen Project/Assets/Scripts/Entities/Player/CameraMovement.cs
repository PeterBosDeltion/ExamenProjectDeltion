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

    private bool playingCoop;
    private Bounds bounds;
    private bool canMove;
    public float cameraResetRadius = 10;
    //public float maxDistanceRadiusFromCenter;

    private void Start()
    {
        if(GameManager.instance.amountOfPlayers <= 1)
        {
            playingCoop = false;
        }
        else
        {
            playingCoop = true;
            canMove = true;
        }
    }

    public void FindPlayerOne()
    {
        foreach (PlayerController player in GameManager.instance.GetPlayers())
        {
            if (player.playerNumber == 0)
                target = player.transform;
        }

        Vector3 newOffset = Camera.main.transform.forward * -distanceOffset;
        newOffset += Camera.main.transform.up * heightOfset;
        transform.position = target.position + newOffset;
    }

    //Either late or fixed update CAN work with this.
    private void FixedUpdate()
    {
        if (focusOnPlayer && !playingCoop)
        {
            MoveAfterPlayer();
        }
        else
        {
            if(canMove)
                MoveAfterCenterpoint();
            KeepPlayersInDistance();
        }
    }

    //Camera follows player
    void MoveAfterPlayer()
    {
        if (target)
        {
            Vector3 newOffset = Camera.main.transform.forward * -distanceOffset;
            newOffset += Camera.main.transform.up * heightOfset;
            Vector3 targetPos = target.position + newOffset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothness * Time.deltaTime);

            transform.localPosition = smoothedPos;
        }
    }

    void MoveAfterCenterpoint()
    {
        if(GameManager.instance.activePlayers.Count > 0)
            bounds = new Bounds(GameManager.instance.activePlayers[0].transform.position, Vector3.zero);
        for (int i = 0; i < GameManager.instance.activePlayers.Count; i++)
        {
            bounds.Encapsulate(GameManager.instance.activePlayers[i].transform.position);
        }
        Vector3 newOffset = Camera.main.transform.forward * -distanceOffset;
        newOffset += Camera.main.transform.up * heightOfset;
        Vector3 targetPos = bounds.center + newOffset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothness * Time.deltaTime);

            transform.localPosition = smoothedPos;
    }

    void KeepPlayersInDistance()
    {
        foreach (var player in GameManager.instance.activePlayers)
        {
           
            Vector3 pos = Camera.main.WorldToViewportPoint(player.transform.position);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);

            if (screenPos.x <= 0 || screenPos.y <= 0 || screenPos.x >= Screen.width || screenPos.y >= Screen.height)
            {
                canMove = false;
                Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                player.transform.position = new Vector3(newPos.x, player.transform.position.y, newPos.z);
               
            }

            Vector3 centerPosition = bounds.center; 
            float distance = Vector3.Distance(player.transform.position, centerPosition); 

            if (distance <= cameraResetRadius) 
            {
                canMove = true;
            }
        }
    }
}