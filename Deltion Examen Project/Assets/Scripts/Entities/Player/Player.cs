using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public delegate void HpEvent();

    public HpEvent zeroTempHp;

    private void Awake()
    {
        hp = maxHp;
        zeroTempHp += EmptyHpEvent;
    }

    private void Start()
    {
        EntityManager.instance.AddPlayerOrAbility(this);
    }

    public void OnDestroy()
    {
        zeroTempHp -= EmptyHpEvent;
        EntityManager.instance.RemovePlayerOrAbility(this);
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