using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeDrone : MonoBehaviour
{
    private Transform target;

    private float myAggroRadius;
    private float myRange;
    private float myDamage;
    private float myAOERadius;
    private Player myPlayer;
    private KamikazeDroneAbility myAbility;
    public GameObject explosionParticle;
    private float myDelay;
    private bool delayed;
    private bool delaying;

    private float mySpeed;

    public void Initialize(float range, float damage, float aggroRadius, float aoeRadius, Player player, KamikazeDroneAbility ability, float droneDelay, float speed)
    {
        myRange = range;
        myDamage = damage;
        myAggroRadius = aggroRadius;
        myAOERadius = aoeRadius;
        myPlayer = player;
        myAbility = ability;
        myDelay = droneDelay;
        mySpeed = speed;
        if (!delaying)
        {
            StartCoroutine(Delay());
        }
    }

    private void FixedUpdate()
    {
        if (target && delayed)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, mySpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) <= .5F)
            {
                Explode();
            }
        }
    }

    private IEnumerator Delay()
    {
        delaying = true;
        yield return new WaitForSeconds(myDelay);
        delayed = true;
        delaying = false;
    }

    private void Update()
    {
        if (!target)
        {
            FindTarget();
        }
        else
        {
            transform.LookAt(target);
        }
    }
    private void FindTarget()
    {
        float closestDistance = Mathf.Infinity;
        Collider closest = null;
        if (!target)
        {
            Collider[] overlaps = Physics.OverlapSphere(transform.position, myAggroRadius);
            if (overlaps.Length > 0)
            {
                
                foreach (var collider in overlaps)
                {
                    if (collider.transform.GetComponent<Enemy>())
                    {
                        if(closestDistance > Vector3.Distance(transform.position, collider.transform.position))
                        {
                            closestDistance = Vector3.Distance(transform.position, collider.transform.position);
                            closest = collider;
                        }
                    }
                }

                if (closest)
                {
                    target = closest.transform;
                }
            }
        }

    }

    public void Explode()
    {
        myAbility.CheckDrones();
        explosionParticle.SetActive(true);
        target.GetComponent<Enemy>().TakeDamage(myDamage, null);
        Destroy(gameObject, 1F);
    }
}
