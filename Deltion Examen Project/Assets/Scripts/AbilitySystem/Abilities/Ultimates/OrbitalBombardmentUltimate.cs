using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBombardmentUltimate : MonoBehaviour
{
    private float myClipLenght;
    public GameObject explosionParticle;

    private bool waiting;
    private float myDmgRadius;
    private float myDmg;
    private Player myPlayer;
    private AudioSource source;
    public void Initialize(float damageRadius, float damage, Player player)
    {
        myClipLenght = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        myDmg = damage;
        myDmgRadius = damageRadius;
        myPlayer = player;
        if (!waiting)
        {
            StartCoroutine(TimeTillDamage());
        }
        source = GetComponent<AudioSource>();
        GetComponentInChildren<SpriteRenderer>().color = GameManager.instance.playerColors[myPlayer.GetComponent<PlayerController>().playerNumber];
    }

    private IEnumerator TimeTillDamage()
    {
        waiting = true;
        yield return new WaitForSeconds(myClipLenght);
        GameObject e = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        e.transform.localScale *= 1.5F;
        Collider[] overlaps = Physics.OverlapSphere(transform.position, myDmgRadius);
        foreach (var col in overlaps)
        {
            if (col.GetComponent<Enemy>())
            {
                    col.GetComponent<Enemy>().TakeDamage(myDmg, myPlayer);
            }
        }

        source.PlayOneShot(source.clip);
        Destroy(e, 1.0F);
        Destroy(gameObject, 1.0F);
    }
}
