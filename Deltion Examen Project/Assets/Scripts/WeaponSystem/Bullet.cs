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

    public Player myPlayer;

    public void Initialize(float newDamage, float minDrop, float maxDrop, Vector3 originPosition, Player player)
    {
        startDamage = newDamage;
        damage = startDamage;
        minFalloff = minDrop;
        maxFalloff = maxDrop;
        originPos = originPosition;
        myPlayer = player;
    }
    private void Update()
    {
        distanceTraveled = Vector3.Distance(transform.position, originPos);
        if(distanceTraveled < maxFalloff)
        {
            if (distanceTraveled >= minFalloff)
            {
                damage = startDamage / 1.5F;
            }
        }
        else if(distanceTraveled >= maxFalloff)
        {
            damage = startDamage / 3F;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.GetComponent<Entity>())
        {
            Entity entity = collision.transform.gameObject.GetComponent<Entity>();
            entity.TakeDamage(damage, myPlayer);
        }

        Destroy(this.gameObject);
    }
}
