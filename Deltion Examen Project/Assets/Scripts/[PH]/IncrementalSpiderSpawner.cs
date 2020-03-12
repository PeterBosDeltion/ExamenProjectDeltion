using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalSpiderSpawner : MonoBehaviour
{
    public int amountPerTick;
    public float tickTime;
    public GameObject Spider;

    private void Start()
    {
        StartCoroutine(SpawnSpiders(3));
    }

    private IEnumerator SpawnSpiders(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < amountPerTick; i++)
        {
            Instantiate(Spider, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(SpawnSpiders(tickTime));
    }
}
