using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void OnEntityDeath();
    public OnEntityDeath deathEvent;

    public delegate void TakeDamageEvent();
    public TakeDamageEvent takeDamageEvent;
    protected float hp;
    protected float tempHp;
    [HideInInspector]
    public float maxTempHp;
    public float maxHp;

    [HideInInspector]
    public bool death;

    public float xpWhenKilled = 0;

    protected virtual void Awake()
    {
        deathEvent += EmptyDeathEvent;
        takeDamageEvent += EmptyDeathEvent;
    }

    protected virtual void Start()
    {
        SetEntityValues();
    }

    protected virtual void OnDestroy()
    {
        EntityManager.instance.RemovePlayerOrAbility(this);
        deathEvent = null;
        takeDamageEvent = null;
    }

    public virtual void SetEntityValues()
    {
        hp = maxHp;
    }

    public void TakeDamage(float takenDamage, Entity Attacker)
    {
        if(!death)
        {
            if (tempHp > 0)
            {
                tempHp -= takenDamage;

                if (tempHp < 0)
                {
                    hp += tempHp;
                    RemoveTempHP();
                }
            }
            else
            {
                hp -= takenDamage;
            }

            if (hp <= 0 && !death)
            {
                Death();
                return;
            }
            DamageEvent(Attacker);

            takeDamageEvent.Invoke();
        }
    }
    public void Heal(float healedHp, float AddedTempHP)
    {
        if(hp > 0)
        {
            hp += healedHp;
            if (hp > maxHp)
                hp = maxHp;
        }
        if(AddedTempHP > 0)
        {
            tempHp = AddedTempHP;
            maxTempHp = AddedTempHP;
        }
    }

    protected virtual void DamageEvent(Entity Attacker)
    {
    }

    public virtual void HealEvent()
    {
    }

    protected virtual void Death()
    {
        TriggerDeathEvents();
        ExperienceManager.instance.AwardExp(xpWhenKilled);
        //Add score when script is made
    }

    public virtual void TriggerDeathEvents()
    {
        death = true;
        deathEvent.Invoke();
    }

    public void RemoveTempHP()
    {
        tempHp = 0;
    }

    public float GetHp()
    {
        return hp;
    }

    public float GetTempHp()
    {
        return tempHp;
    }

    public float GetMaxTempHp()
    {
        return maxTempHp;
    }

    public void EmptyDeathEvent()
    {
    }
}