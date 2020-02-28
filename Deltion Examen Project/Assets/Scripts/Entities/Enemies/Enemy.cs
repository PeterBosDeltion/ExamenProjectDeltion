using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyAI myAI;
    protected Animator anim;
    protected float speed;
    protected float damage;
    protected float attackRange;

    private void Start()
    {
        hp = maxHp;
        anim = GetComponentInChildren<Animator>();
    }

    public override void DamageEvent(Entity Attacker)
    {
        myAI.SetTarget(Attacker);
    }

    protected override void Death()
    {
        anim.SetTrigger("Death");
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    public IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}