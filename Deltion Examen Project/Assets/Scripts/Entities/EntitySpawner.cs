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
        yield return new WaitForSeconds(timeBetweenSpawns);

        Instantiate(entity, transform.position, Quaternion.identity);

        if (que.Count != 0)
        {
            StartCoroutine(EntitySpawning(que[0]));
            que.RemoveAt(0);
        }
        else
        {
            spawning = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}