using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moab : MonoBehaviour
{
    private float myDmg;
    private float myDmgRadius;
    private Player myPlayer;
    private bool waiting;
    private AudioSource source;
    private MoabAbility myAbility;

    public GameObject aoePrefab;
    public ParticleSystem nukeExplosion;
    private RaycastHit hit;
    public LayerMask floor;
    private bool exploded;
    public void Initialize(float damageRadius, float damage, Player player, MoabAbility ability)
    {
        myDmg = damage;
        myDmgRadius = damageRadius;
        myPlayer = player;
     
        source = GetComponent<AudioSource>();
        myAbility = ability;

    }

    private void OnCollisionEnter(Collision col)
    {
        if(!exploded)
            Explode();
    }

    private void Explode()
    {
        exploded = true;
        GetComponentInChildren<Renderer>().enabled = false;
        nukeExplosion.Play();
        Collider[] overlaps = Physics.OverlapSphere(transform.position, myDmgRadius);
        foreach (var col in overlaps)
        {
            if (col.GetComponent<Enemy>())
            {
                col.GetComponent<Enemy>().TakeDamage(myDmg, myPlayer);
            }
        }

        source.PlayOneShot(source.clip);
       
        if (!waiting)
        {
            StartCoroutine(WaitForParticle());
        }
    }

    private IEnumerator WaitForParticle()
    {
        waiting = true;
        myAbility.spawnedAOE = Instantiate(aoePrefab, transform.position, Quaternion.identity);
        myAbility.spawnedAOE.GetComponent<MoabGas>().Initialize(myAbility.aoeDamage, myPlayer, myAbility.aoeRadius);
        yield return new WaitForSeconds(nukeExplosion.main.duration);
        Destroy(gameObject);
        waiting = false;
    }
}
