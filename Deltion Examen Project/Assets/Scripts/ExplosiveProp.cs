using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProp : MonoBehaviour
{
    public float damage;
    public float damageRadius;

    public GameObject explosionParticlePrefab;
    public ParticleSystem fireParticle;

    public bool explodeOnTrigger;

    private bool waiting;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (explodeOnTrigger)
        {
            if (other.GetComponent<Entity>())
            {
                Explode();
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!explodeOnTrigger)
        {
            if (collision.transform.GetComponent<Bullet>())
            {
                if (!waiting)
                    StartCoroutine(BurnExplode());
            }
        }
    }

    private IEnumerator BurnExplode()
    {
        waiting = true;
        fireParticle.Play();
        yield return new WaitForSeconds(1F);
        fireParticle.Stop();
        Explode();
    }

    public void Explode()
    {
        Collider[] hitEnts = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hit in hitEnts)
        {
            if (hit.GetComponent<Entity>())
                hit.GetComponent<Entity>().TakeDamage(damage, null);
        }

        GameObject expl = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(source.clip, transform.position);
        Destroy(expl, 2.0F);
        Destroy(gameObject);
    }
}
