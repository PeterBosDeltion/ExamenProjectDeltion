using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public bool objectiveSpawner;

    [HideInInspector]
    public float timeBetweenSpawns;
    private bool spawning;
    private List<GameObject> que = new List<GameObject>();

    private Color GizmoColor = Color.red;
    [HideInInspector]
    public bool EntityToClose;

    private int amountOfEntitysToClose;

    private void Start()
    {
        if(!objectiveSpawner)
            GetComponent<SphereCollider>().radius = LevelManager.instance.NoSpawnsDistance;
    }

    public void AddToSpawnQue(GameObject entity)
    {
        que.Add(entity);
        if(!spawning)
        {
            spawning = true;
            StartCoroutine(EntitySpawning(que[0]));
            que.RemoveAt(0);
        }
    }

    private IEnumerator EntitySpawning(GameObject entity)
    {
        GizmoColor = Color.cyan;

        yield return new WaitForSeconds(timeBetweenSpawns);

        if (!EntityToClose)
            Instantiate(entity, transform.position, Quaternion.identity);
        else if (!objectiveSpawner)
            LevelManager.instance.ReasignEnemys(entity);
        else
            que.Add(entity);

        if (que.Count != 0)
        {
            StartCoroutine(EntitySpawning(que[0]));
            que.RemoveAt(0);
        }
        else
        {
            GizmoColor = Color.red;
            spawning = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;

        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Entity>())
        {
            if (!EntityToClose)
                EntityToClose = true;

            amountOfEntitysToClose++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Entity>())
        {
            amountOfEntitysToClose--;

            if (amountOfEntitysToClose == 0 && !EntityToClose)
                EntityToClose = false;
        }
    }
}