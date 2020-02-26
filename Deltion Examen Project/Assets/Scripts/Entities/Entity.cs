using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void HpEvent(Entity Attacker = null);
    public HpEvent hpDamaged;
    public HpEvent tempHpDamaged;

    protected float hp;
    public float maxHp;
    protected float tempHp;

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

    protected void Death()
    {
        Destroy(this.gameObject);
        //Add score when script is made
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

    public void RemoveTempHP()
    {
        tempHp = 0;
    }
}