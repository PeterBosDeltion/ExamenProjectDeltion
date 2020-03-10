using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
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
    public float TimeBetweenAttacks;

    protected AudioSource mainAudioSource;
    public AudioClip attackClip;
    public AudioClip deathClip;
    public AudioClip hitClip;

    private void Awake()
    {
        myStats = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        mainAudioSource = GetComponent<AudioSource>();
        myStats.myAI = this;
    }

    private void Start()
    {
        entityManager = EntityManager.instance;
        entityManager.AddEnemy(myStats);
        SetTarget();
        agent.speed = myStats.speed;
    }

    private void Update()
    {
        if (!agent.isStopped && myTarget)
        {
            ManageDestination();
        }
        if (myTarget)
        {
            distanceToTarget = Vector3.Distance(transform.position, myTarget.transform.position);
            float distanceTillRotate = distanceToTarget - 1;
            if (distanceTillRotate <= myStats.attackRange && state != AIState.Dead)
            {
                agent.updateRotation = false;
                Quaternion targetRotation = Quaternion.LookRotation(myTarget.transform.position - transform.position, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
            }
            else
            {
                agent.updateRotation = true;
            }
        }

        HandelAI();
    }

    public void SetAndPlayAudioClipOnce(AudioClip clip, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }
    private void PlayAudioClipLoop(bool On)
    {
        mainAudioSource.loop = true;
        if (On)
            mainAudioSource.Play();
        else
            mainAudioSource.Stop();
    }
    private void OnDestroy()
    {
        entityManager.RemoveEnemy(myStats);
    }

    //This function is used to set a new target
    public void SetTarget(Entity Attacker = null)
    {
        StopAllCoroutines();
        if (state != AIState.Dead)
        {
            if (!Focused && Attacker)
            {
                myTarget = Attacker;
                SetState(AIState.ClosingIn);
                StartCoroutine(LockedOnTimer());
            }
            else if (!myTarget)
            {
                myTarget = GetClosestTarget();
                if (myTarget == null)
                {
                    //Posibly need to call this more than once before setting it to idle
                    if (state != AIState.Idle)
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
        if (entityManager.AllPlayersAndAbilities.Count != 0)
        {
            Entity closestEntity = null;
            float rangeToClosest = Mathf.Infinity;
            foreach (Entity entity in entityManager.AllPlayersAndAbilities)
            {
                float newDistance = Vector3.Distance(transform.position, entity.transform.position);
                if (rangeToClosest > newDistance)
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
        state = newState;
        HandleAIStates();
        SetAnimation();
    }

    //This function hadels generic actions that need to happen on changing a state
    protected virtual void HandleAIStates()
    {
        switch (state)
        {
            case AIState.Idle:
                agent.isStopped = true;
                PlayAudioClipLoop(false);
                SetTarget();
                break;
            case AIState.ClosingIn:
                agent.isStopped = false;
                PlayAudioClipLoop(true);
                break;
            case AIState.Attacking:
                agent.isStopped = true;
                Attack();
                GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                SetAndPlayAudioClipOnce(attackClip);
                PlayAudioClipLoop(false);
                break;
            case AIState.BackingOff:
                agent.isStopped = false;
                PlayAudioClipLoop(true);
                break;
            case AIState.Dead:
                agent.isStopped = true;
                PlayAudioClipLoop(false);
                SetAndPlayAudioClipOnce(deathClip);
                break;
        }
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

    private void ManageDestination()
    {
        if(distanceToTarget <= myStats.attackRange + 3)
        {
            UpdateDestination();
        }
    }
    
    //This function is used to move the enemy towards it target if its not stopped and has a target
    public virtual void UpdateDestination()
    {
        if(myTarget)
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
}