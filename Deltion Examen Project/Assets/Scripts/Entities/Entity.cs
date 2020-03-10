using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float hp;
    protected float tempHp;
    [HideInInspector]
    public float maxTempHp;
    public float maxHp;

    public bool death;

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

            if (hp <= 0)
            {
                death = true;
                Death();
            }
            DamageEvent(Attacker);
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

    public virtual void DamageEvent(Entity Attacker)
    {

    }

    public virtual void HealEvent()
    {

    }

    protected virtual void Death()
    {
        Destroy(this.gameObject);
        //Add score when script is made
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

    public void EmptyHpEvent(Entity Attacker = null)
    {
    }
}