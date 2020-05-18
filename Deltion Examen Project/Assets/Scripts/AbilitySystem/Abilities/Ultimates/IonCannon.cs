using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonCannon : MonoBehaviour
{
    public float chaseSpeed = 10;
    private float myDmg;
    private Player myPlayer;
    private bool canDamage;
    private bool waiting;
    private float myTickInterval;

    private Transform target;

    public void Initialize(float damage, Player player, float damageTickInterval)
    {
        myDmg = damage;
        myPlayer = player;
        myTickInterval = damageTickInterval;
        target = GetClosestTarget().transform;
        canDamage = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy e = other.GetComponent<Enemy>();
            if (canDamage)
            {
                e.TakeDamage(myDmg, myPlayer);
                if (!waiting)
                {
                    StartCoroutine(ResetCanDamage());
                }
                if(e.GetHp() <= 0)
                {
                    target = null;
                }
            }
        }
    }

    private void Update()
    {
        if (target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
        }
        else
        {
            Entity e = GetClosestTarget();
            if(e)
                target = e.transform;
        }
    }

    private Entity GetClosestTarget()
    {
        if (EntityManager.instance.AllEnemys.Count != 0)
        {
            Entity closestEntity = null;
            float rangeToClosest = Mathf.Infinity;
            foreach (Entity entity in EntityManager.instance.AllEnemys)
            {
                float newDistance = Vector3.Distance(transform.position, entity.transform.position);
                if (rangeToClosest > newDistance && entity.enabled && !entity.death)
                {
                    rangeToClosest = newDistance;
                    closestEntity = entity;
                }
            }

            return closestEntity;
        }
        else
        {
            return null;
        }
    }

    private IEnumerator ResetCanDamage()
    {
        waiting = true;
        canDamage = false;
        yield return new WaitForSeconds(myTickInterval);
        canDamage = true;
        waiting = false;

    }
}
