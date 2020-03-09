using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    //This is the actual range a melle enemy will attack at. This is to give it a better change to hit before the target moves out of the normal Attack range
    public float attackRangeMelee;
    public AnimationClip myAttack;
    [Tooltip("this value refrences how far into the attack animation the player takes damage (0.1 would be 10% into the animation)")]
    public float timeBeforeDamage;

    protected override void HandelAI()
    {
        if (state == AIState.ClosingIn)
        {
            if(distanceToTarget <= attackRangeMelee)
            {
                SetState(AIState.Attacking);
            }
        }
        if(state == AIState.Attacking)
        {
            if(distanceToTarget > myStats.attackRange)
            {
                SetState(AIState.ClosingIn);
                StopAllCoroutines();
            }
        }
    }

    protected override void Attack()
    {
        StartCoroutine(TimeTillAttack());
    }

    protected IEnumerator TimeTillAttack()
    {
        yield return new WaitForSeconds(myAttack.length * timeBeforeDamage);
        if (distanceToTarget <= myStats.attackRange)
        {
            myTarget.TakeDamage(myStats.damage, myStats);
        }
        yield return new WaitForSeconds(TimeBetweenAttacks);
        if (distanceToTarget <= myStats.attackRange)
        {
            HandleAIStates();
            SetAnimation();
        }
    }
}