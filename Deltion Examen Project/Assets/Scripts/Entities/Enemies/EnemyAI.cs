using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public abstract class EnemyAI : MonoBehaviour
{
    protected enum AIState
    {
        Idle,
        ClosingIn,
        Attacking,
        BackingOff
    }

    protected AIState state;

    protected Enemy myStats;
    protected Entity myTarget;
    public float AttantionTime;
    protected bool Focused;
    private EntityManager entityManager;

    private void Awake()
    {
        myStats = GetComponent<Enemy>();
        myStats.hpDamaged += SetTarget;
    }

    private void Start()
    {
        entityManager = EntityManager.instance;
        entityManager.AddEnemy(myStats);
        SetTarget();
        //Set target seems to happen to early
    }

    protected void SetTarget(Entity Attacker = null)
    {
        if (!Focused && Attacker)
        {
            myTarget = Attacker;
            StartCoroutine(LockedOnTimer());
        }
        else
        {
            myTarget = GetClosestTarget();
            if(myTarget = null)
            {
                //Posibly need to call this more than once before setting it to idle
                state = AIState.Idle;
            }
        }
    }

    private Entity GetClosestTarget()
    {
        if(entityManager.AllPlayersAndAbilitys.Count != 0)
        {
            Entity closestEntity = null;
            float rangeToClosest = Mathf.Infinity;
            foreach(Entity entity in entityManager.AllPlayersAndAbilitys)
            {
                float newDistance = Vector3.Distance(transform.position, entity.transform.position);
                if(rangeToClosest > newDistance)
                {
                    rangeToClosest = newDistance;
                    closestEntity = entity;
                }
            }

            return closestEntity;
        }
        else
        {
            return null;
        }
    }

    protected abstract void HandleAI();

    protected abstract void Attack();

    private IEnumerator LockedOnTimer()
    {
        Focused = true;
        yield return new WaitForSeconds(AttantionTime);
        Focused = false;
    }
}