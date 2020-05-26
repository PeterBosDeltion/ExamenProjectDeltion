﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float startDamage;
    private float damage;
    private float minFalloff;
    private float maxFalloff;
    private float distanceTraveled;
    private float myAoeRadius;
    private bool exploded;
    private Vector3 originPos;

    public Entity myEnt;
    private RaycastHit hit;
    public GameObject bloodParticle;
    public GameObject DebrisParticle;

    public AudioClip impactSound;
    private bool isPiercing;
    private bool incremented;

    private int maxPierce;
    private int currentPierce;
    private float pierceDivisionValue;
    private List<Entity> piercedEnts = new List<Entity>();
    public void Initialize(float newDamage, float minDrop, float maxDrop, Vector3 originPosition, Entity damagingEntity, float aoeRadius = 0, bool piercing = false, int maxPierceAmount = 0, float pierceDivision = 0)
    {
        startDamage = newDamage;
        damage = startDamage;
        minFalloff = minDrop;
        maxFalloff = maxDrop;
        originPos = originPosition;
        myEnt = damagingEntity;

        if(aoeRadius > 0)
        {
            myAoeRadius = aoeRadius;
        }

        isPiercing = piercing;
        maxPierce = maxPierceAmount;
        pierceDivisionValue = pierceDivision;
        currentPierce = 0;
        piercedEnts.Clear();
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
        if (!isPiercing)
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (impactSound)
        {
            PlayImpactSound();
        }

        if (collision.transform.gameObject.GetComponent<Entity>())
        {
           
            if (myAoeRadius <= 0)
            {

                Entity entity = collision.transform.gameObject.GetComponent<Entity>();
                if(entity.GetHp() > 0)
                {
                    entity.TakeDamage(damage, myEnt);
                    if (entity.GetHp() <= 0 && !incremented)
                    {
                        if (myEnt.GetComponent<PlayerController>())
                        {
                            myEnt.GetComponent<PlayerController>().ultimateAbility.IncrementUltCharge();
                            incremented = true;
                        }

                        if (myEnt.GetComponent<SentryTurret>())
                        {
                            myEnt.GetComponent<SentryTurret>().myPlayer.GetComponent<PlayerController>().ultimateAbility.IncrementUltCharge();
                            incremented = true;
                        }
                    }
                  
                }

                if(collision.gameObject.GetComponent<Enemy>())
                {
                    Vector3 bulletTrajectory = collision.transform.position - collision.GetContact(0).point;
                    bulletTrajectory.z = 1;
                    Ray newRay = new Ray(collision.GetContact(0).point, bulletTrajectory);


                    if (Physics.Raycast(newRay, out hit))
                    {
                        Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(collision.GetContact(0).point, transform.up - collision.transform.position));
                    }
                }
                else
                {
                    Instantiate(DebrisParticle, transform.position, Quaternion.LookRotation(collision.GetContact(0).point, transform.up - collision.transform.position));
                }
            }
            else
            {
                if (!exploded)
                {
                    Explode();
                }
            }

            if(!isPiercing)
                Destroy(this.gameObject);

        }

        

        if(!collision.transform.gameObject.GetComponent<Entity>() && !collision.transform.gameObject.GetComponentInParent<Entity>() && !collision.transform.gameObject.GetComponentInChildren<Entity>())
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().isTrigger)
        {
            return;
        }
        if (!isPiercing)
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (impactSound)
        {
            PlayImpactSound();
        }

     

        if (myAoeRadius <= 0)
        {
            Entity entity = null;
            if (other.transform.gameObject.GetComponent<Entity>())
                    entity = other.transform.gameObject.GetComponent<Entity>();
            if (other.transform.gameObject.GetComponentInParent<Entity>())
                entity = other.transform.gameObject.GetComponentInParent<Entity>();
            if (other.transform.gameObject.GetComponentInChildren<Entity>())
                entity = other.transform.gameObject.GetComponentInChildren<Entity>();

            if (entity)
            {
                if (entity.GetHp() > 0)
                {
                    if (!piercedEnts.Contains(entity))
                    {
                        entity.TakeDamage(damage, myEnt);
                        piercedEnts.Add(entity);
                    }

                    if (entity.GetHp() <= 0 && !incremented)
                    {
                        if (myEnt.GetComponent<PlayerController>())
                        {
                            myEnt.GetComponent<PlayerController>().ultimateAbility.IncrementUltCharge();
                            incremented = true;
                        }
                    }
                }

                Vector3 bulletTrajectory = other.transform.position - transform.position;
                bulletTrajectory.z = 1;
                Ray newRay = new Ray(transform.position, bulletTrajectory);


                if (Physics.Raycast(newRay, out hit))
                {
                    Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(transform.position, transform.up - other.transform.position));
                }
            }
        }
        else
        {
            if (!exploded)
            {
                Explode();
            }
        }

        if (!isPiercing)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (currentPierce < maxPierce)
            {
                currentPierce++;
                damage /= pierceDivisionValue;
            }
            else if (currentPierce >= maxPierce)
            {
                Destroy(this.gameObject);
            }
        }

    if (!other.transform.gameObject.GetComponent<Entity>() && !other.transform.gameObject.GetComponentInParent<Entity>() && !other.transform.gameObject.GetComponentInChildren<Entity>())
        Destroy(this.gameObject);
    }

        private void PlayImpactSound()
    {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
    }

    private void Explode()
    {
        exploded = true;
        List<Entity> incrementedEnts = new List<Entity>();
        List<Entity> damagedEnts = new List<Entity>();
        incrementedEnts.Clear();
        damagedEnts.Clear();
        Collider[] overlaps = Physics.OverlapSphere(transform.position, myAoeRadius);
        foreach (var col in overlaps)
        {
            Entity targetEnt = null;
            if (col.transform.gameObject.GetComponent<Entity>())
                targetEnt = col.transform.gameObject.GetComponent<Entity>();
            if (col.transform.gameObject.GetComponentInParent<Entity>())
                targetEnt = col.transform.gameObject.GetComponentInParent<Entity>();
            if (col.transform.gameObject.GetComponentInChildren<Entity>())
                targetEnt = col.transform.gameObject.GetComponentInChildren<Entity>();

            if (targetEnt)
            {
                if (targetEnt.GetHp() > 0)
                {
                    if (!damagedEnts.Contains(targetEnt))
                    {
                        targetEnt.TakeDamage(damage, myEnt);
                        damagedEnts.Add(targetEnt);
                    }
                    if (targetEnt.GetHp() <= 0 && !incrementedEnts.Contains(targetEnt))
                    {
                        myEnt.GetComponent<PlayerController>().ultimateAbility.IncrementUltCharge();
                        incrementedEnts.Add(targetEnt);
                    }
                }
            }
          
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            GameObject expl = Instantiate(bloodParticle, hit.point, Quaternion.identity); //Blood particle is explosion in this case
            Destroy(expl, 2F);
        }
     

    }
}
