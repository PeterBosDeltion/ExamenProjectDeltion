using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void HpEvent(Entity Attacker = null);
    public static HpEvent hpDamaged;
    public static HpEvent tempHpDamaged;

    protected float hp;
    protected float tempHp;
    public float maxHp;

    private void Awake()
    {
        hpDamaged += EmptyHpEvent;
        tempHpDamaged += EmptyHpEvent;
    }

    private void OnDestroy()
    {
        hpDamaged -= EmptyHpEvent;
        tempHpDamaged -= EmptyHpEvent;
    }

    public void TakeDamage(float takenDamage, Entity Attacker)
    {
        if (tempHp > 0)
        {
            tempHp -= takenDamage;

            tempHpDamaged.Invoke(Attacker);

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

        hpDamaged.Invoke(Attacker);

        if (hp <= 0)
        {
            Death();
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
        }
    }

    protected void Death()
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

    public void EmptyHpEvent(Entity Attacker = null)
    {
    }
}