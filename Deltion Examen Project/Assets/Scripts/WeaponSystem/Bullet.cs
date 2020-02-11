using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float startDamage;
    private float damage;
    private float minFalloff;
    private float maxFalloff;
    private float distanceTraveled;
    private Vector3 originPos;

    public void Initialize(float damage, float minDrop, float maxDrop, Vector3 originPosition)
    {
        startDamage = damage;
        damage = startDamage;
        minFalloff = minDrop;
        maxFalloff = maxDrop;
        originPos = originPosition;
    }
    private void Update()
    {
        distanceTraveled = Vector3.Distance(transform.position, originPos);
        if(distanceTraveled < maxFalloff)
        {
            if (distanceTraveled >= minFalloff)
            {
                damage = startDamage / 1.5F;
                Debug.Log("Min");
            }
        }
        else if(distanceTraveled >= maxFalloff)
        {
            damage = startDamage / 3F;
            Debug.Log("Max");
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.transform.gameObject.GetComponent<Enemy>())
        //{
        //    Enemy emy = collision.transform.gameObject.GetComponent<Enemy>();
        //    emy.TakeDamage(damage);
        //}
    }
}
