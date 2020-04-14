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

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponentInChildren<Animator>();
    }

    public override void SetEntityValues()
    {
        base.SetEntityValues();
        damage *= LevelManager.instance.damageModifier;
    }

    protected override void DamageEvent(Entity Attacker)
    {
        myAI.SetTarget(Attacker);
        myAI.SetAndPlayAudioClipOnce(myAI.hitClip,0.1f);
        if (Attacker)
        {
            if (Attacker.GetComponent<PlayerController>())
            {
                Attacker.GetComponent<PlayerController>().ultimateAbility.IncrementUltCharge();
            }
        }
       
    }

    protected override void Death()
    {
        myAI.SetState(EnemyAI.AIState.Dead);
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    public IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(1.5F);
        Destroy(this.gameObject);
    }
}