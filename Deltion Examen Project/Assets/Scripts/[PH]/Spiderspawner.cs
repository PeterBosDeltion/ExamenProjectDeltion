using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiderspawner : MonoBehaviour
{
    private List<GameObject> spooders = new List<GameObject>();
    public int desiredAmountOfSpooders;
    private int currentAmountOfSpooders;
    public List<Vector3> spawnPosses = new List<Vector3>();

    public GameObject spooderPrefab;
    private void Update()
    {
        if(currentAmountOfSpooders < desiredAmountOfSpooders)
        {
           GameObject spood = Instantiate(spooderPrefab, spawnPosses[Random.Range(0, spawnPosses.Count - 1)], Quaternion.identity);
            currentAmountOfSpooders++;
            spooders.Add(spood);
        }

        for (int i = 0; i < spooders.Count -1; i++)
        {
            if (!spooders[i])
            {
                spooders.Remove(spooders[i]);
                currentAmountOfSpooders--;
            }
        }
    }


}
