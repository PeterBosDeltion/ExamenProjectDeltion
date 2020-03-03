using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeDrone : MonoBehaviour
{
    private Transform target;

    private float myAggroRadius;
    private float myDamage;
    private float myAOERadius;
    private Player myPlayer;
    private KamikazeDroneAbility myAbility;
    public GameObject explosionParticle;
    private bool delayed;
    private bool delaying;
    private bool followPlayer;

    private float mySpeed;
    private AudioSource audioSource;
    public AudioClip explosionSFX;

    public float followZOffset = -0.7F;
    public float followYOffset = 2.5F;

    private bool exploding;
    public void Initialize(float damage, float aggroRadius, float aoeRadius, Player player, KamikazeDroneAbility ability, float speed)
    {
        myDamage = damage;
        myAggroRadius = aggroRadius;
        myAOERadius = aoeRadius;
        myPlayer = player;
        myAbility = ability;
        mySpeed = speed;
        audioSource = GetComponent<AudioSource>();

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
                if (!exploding)
                {
                    Explode();
                }
            }
        }

        if (followPlayer && !target)
        {
            Vector3 targetPos = myPlayer.transform.position + myPlayer.transform.forward * followZOffset + new Vector3(0, followYOffset, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, mySpeed * Time.deltaTime);
            Vector3 lookPos = myPlayer.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * mySpeed);
        }
    }

    private IEnumerator Delay()
    {
        delaying = true;
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
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
            if (delayed)
            {
                transform.LookAt(target);
            }
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
                    int amountEnemies = 0;
                    if (collider.transform.GetComponent<Enemy>())
                    {
                        if(collider.transform.GetComponent<Enemy>().GetHp() > 0)
                        {
                            amountEnemies++;
                            if (closestDistance > Vector3.Distance(transform.position, collider.transform.position))
                            {
                                closestDistance = Vector3.Distance(transform.position, collider.transform.position);
                                closest = collider;
                            }
                        }
                    }

                    if(amountEnemies <= 0)
                    {
                        followPlayer = true;
                    }
                    else
                    {
                        followPlayer = false;
                    }
                }

                if (closest)
                {
                    if(closest.GetComponent<Enemy>().GetHp() > 0)
                    {
                        target = closest.transform;
                    }
                }
            }
        }

    }

    public void Explode()
    {
        exploding = true;
        myAbility.CheckDrones();
        explosionParticle.SetActive(true);
        target.GetComponent<Enemy>().TakeDamage(myDamage, myPlayer);
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(explosionSFX);

        Collider[] overlaps = Physics.OverlapSphere(transform.position, myAOERadius);
        foreach (var col in overlaps)
        {
            if (col.GetComponent<Enemy>())
            {
                if(col.transform != target)
                {
                    col.GetComponent<Enemy>().TakeDamage(myDamage, myPlayer);
                }
            }
        }
        Destroy(gameObject, 1F);
    }
}
