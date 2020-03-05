using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyAI myAI;
    public Animator anim;
    public float speed;
    public float damage;
    public float attackRange;

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
        myAI.SetState(EnemyAI.AIState.Dead);
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    public IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}