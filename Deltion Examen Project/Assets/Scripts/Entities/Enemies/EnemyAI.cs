using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy),typeof(NavMeshAgent))]
public abstract class EnemyAI : MonoBehaviour
{
    public delegate void StateEvent();

    public StateEvent StateChanged;

    public enum AIState
    {
        Idle,
        ClosingIn,
        Attacking,
        BackingOff,
        Dead
    }

    protected AIState state;

    protected Enemy myStats;
    protected Entity myTarget;
    protected NavMeshAgent agent;
    protected bool Focused;
    protected bool Moving;
    public float AttantionTime;
    private EntityManager entityManager;

    private void Awake()
    {
        myStats = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        myStats.myAI = this;
        StateChanged += SetAnimation;
        StateChanged += HandleAI;
    }

    private void Start()
    {
        entityManager = EntityManager.instance;
        entityManager.AddEnemy(myStats);
        SetTarget();
    }

    private void Update()
    {
        if(!agent.isStopped && myTarget)
        {
            UpdateDestination();
        }
    }

    private void OnDestroy()
    {
        entityManager.RemoveEnemy(myStats);
        StateChanged -= SetAnimation;
        StateChanged -= HandleAI;
    }

    public void SetTarget(Entity Attacker = null)
    {
        StopAllCoroutines();
        if(state != AIState.Dead)
        {
            if (!Focused && Attacker)
            {
                myTarget = Attacker;
                SetState(AIState.ClosingIn);
                StateChanged.Invoke();
                StartCoroutine(LockedOnTimer());
            }
            else if(!myTarget)
            {
                myTarget = GetClosestTarget();
                if(myTarget == null)
                {
                    //Posibly need to call this more than once before setting it to idle
                    if(state != AIState.Idle)
                    {
                        SetState(AIState.Idle);
                        StateChanged.Invoke();
                    }
                    StartCoroutine(ReTarget());
                    return;
                }
                SetState(AIState.ClosingIn);
                StateChanged.Invoke();
            }
        }
    }

    private Entity GetClosestTarget()
    {
        if(entityManager.AllPlayersAndAbilities.Count != 0)
        {
            Entity closestEntity = null;
            float rangeToClosest = Mathf.Infinity;
            foreach(Entity entity in entityManager.AllPlayersAndAbilities)
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

    public void SetState(AIState newState)
    {
        state = newState;
    }

    protected void SetAnimation()
    {
        switch (state)
        {
            case AIState.Idle:
                myStats.anim.SetBool("Walking", false);
                break;
            case AIState.ClosingIn:
                myStats.anim.SetBool("Walking", true);
                break;
            case AIState.Attacking:
                myStats.anim.SetTrigger("Attack");
                break;
            case AIState.BackingOff:
                myStats.anim.SetBool("Walking", true);
                break;
            case AIState.Dead:
                myStats.anim.SetTrigger("Death");
                break;
        }
    }

    protected virtual void UpdateDestination()
    {
        agent.SetDestination(myTarget.transform.position);
    }

    protected abstract void HandleAI();

    protected abstract void Attack();


    private IEnumerator ReTarget()
    {
        yield return new WaitForSeconds(3);
        SetTarget();
    }

    private IEnumerator LockedOnTimer()
    {
        Focused = true;
        yield return new WaitForSeconds(AttantionTime);
        Focused = false;
    }
}