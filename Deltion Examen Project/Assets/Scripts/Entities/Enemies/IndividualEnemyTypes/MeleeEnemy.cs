using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    protected override void HandleAI()
    {
        switch (state)
        {
            case AIState.Idle:
                agent.isStopped = true;
                break;
            case AIState.Attacking:
                agent.isStopped = true;
                break;
            case AIState.Dead:
                agent.isStopped = true;
                break;
        }
    }

    protected override void Attack()
    {
    }
}