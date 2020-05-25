﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveTower : Interactable
{
    public GameObject dropPodPrefab;
    public List<GameObject> revivePositions = new List<GameObject>();
    protected override void Interact()
    {
        interacted = true;
        RevivePlayers();
    }

    public void RevivePlayers()
    {

        List<PlayerController> toRevive = new List<PlayerController>();
        foreach (PlayerController player in GameManager.instance.activePlayers)
        {
            if (player.GetIfDeath())
                toRevive.Add(player);
        }

        foreach (PlayerController p in toRevive)
        {
            revivePositions[p.playerNumber].GetComponent<RevivePosition>().myPlayer = p.GetComponent<Player>();
            GameObject dropPod = Instantiate(dropPodPrefab, revivePositions[p.playerNumber].transform.position + new Vector3(0, 30, 0), Quaternion.identity);
            dropPod.GetComponent<Rigidbody>().AddForce(Vector3.down * Physics.gravity.y * 2);
        }


    }
}