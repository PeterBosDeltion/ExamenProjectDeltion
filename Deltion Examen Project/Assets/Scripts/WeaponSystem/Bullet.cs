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
    private RaycastHit hit;
    public GameObject bloodParticle;

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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (collision.transform.gameObject.GetComponent<Entity>())
        {
            Entity entity = collision.transform.gameObject.GetComponent<Entity>();
            entity.TakeDamage(damage, myPlayer);
            Ray newRay = new Ray(collision.GetContact(0).point, collision.transform.position - collision.GetContact(0).point);

            if(Physics.Raycast(newRay ,out hit))
            {
                Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(collision.GetContact(0).point,transform.up - collision.transform.position));
            }
        }

        Destroy(this.gameObject);
    }
}
