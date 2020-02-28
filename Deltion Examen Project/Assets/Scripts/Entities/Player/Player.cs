using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public HpEvent zeroHp;
    public HpEvent zeroTempHp;

    public void Awake()
    {
        tempHpDamaged += CheckTempHp;
    }
    public void Start()
    {
        EntityManager.instance.AddPlayerOrAbility(this);
        hp = maxHp;
    }

    public void CheckTempHp(Entity Attacker)
    {
        if(tempHp <= 0)
        {
            zeroTempHp.Invoke();
        }
    }
}