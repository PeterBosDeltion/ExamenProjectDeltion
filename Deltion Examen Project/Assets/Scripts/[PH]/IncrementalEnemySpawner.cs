using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalEnemySpawner : MonoBehaviour
{
    public int amountPerTick;
    public float tickTime;
    public GameObject spider;

    private void Start()
    {
        StartCoroutine(SpawnSpiders(3));
    }

    private IEnumerator SpawnSpiders(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < amountPerTick; i++)
        {
            Instantiate(spider, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(SpawnSpiders(tickTime));
    }
}