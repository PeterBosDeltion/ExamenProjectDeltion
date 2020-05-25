using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHurt : MonoBehaviour
{
    public float damage = 500;
    public bool damagedOnce;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Entity>())
        {
            if (!damagedOnce)
            {
                other.GetComponent<Entity>().TakeDamage(damage, null);
                damagedOnce = true;
            }

        }
    }
}
