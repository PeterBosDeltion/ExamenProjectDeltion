using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public delegate void HpEvent();

    public HpEvent zeroTempHp;

    public void Start()
    {
        EntityManager.instance.AddPlayerOrAbility(this);
        hp = maxHp;
        zeroTempHp += EmptyHpEvent;
    }

    public void OnDestroy()
    {
        zeroTempHp -= EmptyHpEvent;
    }

    public override void DamageEvent(Entity Attacker)
    {
        if (tempHp <= 0)
            zeroTempHp.Invoke();
    }

    public void EmptyHpEvent()
    {
    }
}