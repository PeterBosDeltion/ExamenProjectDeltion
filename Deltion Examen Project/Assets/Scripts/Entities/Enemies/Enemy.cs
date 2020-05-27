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

    public SkinnedMeshRenderer myRenderer;
    public Material emmisiveMaterial;
    private Material OriginalMaterial;

    public ParticleSystem deathParticle;

    protected override void Awake()
    {
        base.Awake();

        OriginalMaterial = myRenderer.material;
        anim = GetComponentInChildren<Animator>();
    }

    public override void SetEntityValues()
    {
        maxHp *= LevelManager.instance.healthModifier;
        damage *= LevelManager.instance.damageModifier;
        base.SetEntityValues();
    }

    public void SetTemporaryBuffValue(float buffModifier, bool removeBuff)
    {
        if(!death)
        {
            if (!removeBuff)
            {
                myRenderer.material = emmisiveMaterial;
                damage *= buffModifier;
                speed *= buffModifier;
                myAI.UpdateAgentSpeed();
            }
            else
            {
                myRenderer.material = OriginalMaterial;
                damage /= buffModifier;
                speed /= buffModifier;
                myAI.UpdateAgentSpeed();
            }
        }
    }

    protected override void DamageEvent(Entity Attacker)
    {
        if (myAI)
        {
            myAI.SetTarget(Attacker);
            myAI.SetAndPlayAudioClipOnce(myAI.hitClip,0.1f);
        }
    }

    protected override void Death()
    {
        base.Death();

        if (myAI)
            myAI.SetState(EnemyAI.AIState.Dead);

        deathParticle.Play();

        GetComponent<Collider>().enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    public IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(1.5F);
        Destroy(this.gameObject);
    }
}