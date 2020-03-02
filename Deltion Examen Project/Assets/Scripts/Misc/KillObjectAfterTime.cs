using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjectAfterTime : MonoBehaviour
{

    public float timeTillDestroyed;
    void Start()
    {
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(timeTillDestroyed);
        Destroy(gameObject);
    }
}
