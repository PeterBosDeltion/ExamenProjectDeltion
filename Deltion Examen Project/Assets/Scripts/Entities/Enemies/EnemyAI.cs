using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy),typeof(NavMeshAgent))]
public abstract class EnemyAI : MonoBehaviour
{
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

    protected float distanceToTarget;
    public float attackCooldown;
    protected bool canAttack;

    private void Awake()
    {
        myStats = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        myStats.myAI = this;
        canAttack = true;
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
        if(myTarget)
        {
            distanceToTarget = Vector3.Distance(transform.position, myTarget.transform.position);
            Debug.Log(distanceToTarget);
        }

        HandelAI();
    }

    private void OnDestroy()
    {
        entityManager.RemoveEnemy(myStats);
    }

    //This function is used to set a new target
    public void SetTarget(Entity Attacker = null)
    {
        StopAllCoroutines();
        if(state != AIState.Dead)
        {
            if (!Focused && Attacker)
            {
                myTarget = Attacker;
                SetState(AIState.ClosingIn);
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
                    }
                    StartCoroutine(ReTarget());
                    return;
                }
                SetState(AIState.ClosingIn);
            }
        }
    }

    //This function looks for the closest Entity within the EntityManagers "AllPlayersAndAbilities" list
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

    //This function is used to set the AIstate
    public void SetState(AIState newState)
    {
        Debug.Log(state + "To" + newState);
        state = newState;
        HandleAIStates();
    }

    //This function hadels generic actions that need to happen on changing a state
    protected virtual void HandleAIStates()
    {
        switch (state)
        {
            case AIState.Idle:
                agent.isStopped = true;
                break;
            case AIState.ClosingIn:
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.isStopped = true;
                if(canAttack)
                {
                    Attack();
                    StartCoroutine(AttackCooldown());
                }
                break;
            case AIState.BackingOff:
                agent.isStopped = false;
                break;
            case AIState.Dead:
                agent.isStopped = true;
                break;
        }
        SetAnimation();
    }

    //This function sets the animation needed with the AIState
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
                myStats.anim.SetBool("Walking", false);
                break;
            case AIState.BackingOff:
                myStats.anim.SetBool("Walking", true);
                break;
            case AIState.Dead:
                myStats.anim.SetTrigger("Death");
                break;
        }
    }
    
    //This function is used to move the enemy towards it target if its not stopped and has a target
    protected virtual void UpdateDestination()
    {
        agent.SetDestination(myTarget.transform.position);
    }

    protected abstract void HandelAI();

    protected abstract void Attack();

    //This Coroutine is used as a timer for when the enemy doesent have a taget and needs to rtetarget after being idle (incase this ever hapens)
    private IEnumerator ReTarget()
    {
        yield return new WaitForSeconds(3);
        SetTarget();
    }

    //This Coroutine is used as a timer for when a enemy can change their taget if they get hit
    private IEnumerator LockedOnTimer()
    {
        Focused = true;
        yield return new WaitForSeconds(AttantionTime);
        Focused = false;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}