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

    public ParticleSystem spawnParticle;

    private BoxCollider blockCollider;

    private void Start()
    {
        if(!objectiveSpawner)
        {
            float distance = LevelManager.instance.NoSpawnsDistance;
            GetComponent<SphereCollider>().radius = distance;
            distance *= 1.5f;
            blockCollider = GetComponent<BoxCollider>();
            blockCollider.size = new Vector3(distance, distance, distance);
        }
        else
        {
            float distance = LevelManager.instance.NoSpawnsDistance;
            GetComponent<SphereCollider>().radius = distance;
            distance *= 1.5f;
            blockCollider = GetComponent<BoxCollider>();
            blockCollider.size = new Vector3(distance, distance, distance);
        }

        spawnParticle = GetComponentInChildren<ParticleSystem>();
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

        if(!EntityToClose)
        {
            spawnParticle.Play();
            blockCollider.enabled = true;
        }
        else if(!objectiveSpawner)
        {
            LevelManager.instance.ReasignEnemys(entity);
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
            yield break;
        }
        else
        {
            que.Add(entity);
            StartCoroutine(EntitySpawning(que[0]));
            que.RemoveAt(0);
            yield break;
        }

        yield return new WaitForSeconds(1);

        blockCollider.enabled = false;
        Instantiate(entity, transform.position, Quaternion.identity);

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

            other.GetComponent<Entity>().deathEvent += EntityDied;
            amountOfEntitysToClose++;
        }
    }

    //incase a entity dies whilst being to close to the spawner
    private void EntityDied()
    {
        amountOfEntitysToClose--;
        if (amountOfEntitysToClose == 0 && EntityToClose)
            EntityToClose = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Entity>())
        {
            amountOfEntitysToClose--;
            other.GetComponent<Entity>().deathEvent -= EntityDied;
            if (amountOfEntitysToClose == 0 && EntityToClose)
                EntityToClose = false;
        }
    }
}