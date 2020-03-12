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
    private float myAoeRadius;
    private bool exploded;
    private Vector3 originPos;

    public Entity myEnt;
    private RaycastHit hit;
    public GameObject bloodParticle;

    public AudioClip impactSound;

    public void Initialize(float newDamage, float minDrop, float maxDrop, Vector3 originPosition, Entity damagingEntity, float aoeRadius = 0)
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (collision.transform.gameObject.GetComponent<Entity>())
        {
            if(myAoeRadius <= 0)
            {
                Entity entity = collision.transform.gameObject.GetComponent<Entity>();
                entity.TakeDamage(damage, myEnt);
                Ray newRay = new Ray(collision.GetContact(0).point, collision.transform.position - collision.GetContact(0).point);

                if (Physics.Raycast(newRay, out hit))
                {
                    Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(collision.GetContact(0).point, transform.up - collision.transform.position));
                }
            }
            else
            {
                if (!exploded)
                {
                    Explode();
                }
            }
           
        }

        if (impactSound)
        {
            PlayImpactSound();
        }

        Destroy(this.gameObject);
    }

    private void PlayImpactSound()
    {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
    }

    private void Explode()
    {
        exploded = true;
       
        Collider[] overlaps = Physics.OverlapSphere(transform.position, myAoeRadius);
        foreach (var col in overlaps)
        {
            if (col.GetComponent<Enemy>())
            {
                col.GetComponent<Enemy>().TakeDamage(damage, myEnt);
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
