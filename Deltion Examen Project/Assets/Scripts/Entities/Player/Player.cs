using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public delegate void HpEvent();

    public HpEvent zeroTempHp;

    protected override void Awake()
    {
        base.Awake();

        hp = maxHp;
        zeroTempHp += EmptyHpEvent;
    }

    private void Start()
    {
        EntityManager.instance.AddPlayerOrAbility(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

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

    protected override void Death()
    {
        
    }
}