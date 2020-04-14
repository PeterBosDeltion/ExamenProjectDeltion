using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public bool objectiveSpawner;

    private bool spawning;
    private List<GameObject> que = new List<GameObject>();

    public void AddToSpawnQue(GameObject entity, float timeBetweenSpawns)
    {
        que.Add(entity);
        if(!spawning)
        {
            spawning = true;
            StartCoroutine(EntitySpawning(que[0], timeBetweenSpawns));
            que.RemoveAt(0);
        }
    }

    private IEnumerator EntitySpawning(GameObject entity, float time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(entity, transform.position, Quaternion.identity);

        if (que.Count != 0)
        {
            StartCoroutine(EntitySpawning(que[0], time));
            que.RemoveAt(0);
        }
        else
        {
            spawning = false;
        }
    }
}