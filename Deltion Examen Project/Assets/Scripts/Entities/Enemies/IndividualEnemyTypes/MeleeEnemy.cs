using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    //This is the actual range a melle enemy will attack at. This is to give it a better change to hit before the target moves out of the normal Attack range
    public float attackRangeMelee;
    public AnimationClip myAttack;

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
        canAttack = false;
        StartCoroutine(TimeTillAttack());
    }

    protected IEnumerator TimeTillAttack()
    {
        yield return new WaitForSeconds(myAttack.length);
        if(distanceToTarget <= myStats.attackRange)
        {
            myTarget.TakeDamage(myStats.damage, myStats);
            HandleAIStates();
        }
    }
}