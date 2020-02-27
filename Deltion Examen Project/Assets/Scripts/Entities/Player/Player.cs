using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public HpEvent zeroHp;
    public HpEvent zeroTempHp;

    public List<Ability> abilities = new List<Ability>();
    public Weapon primary;

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

    private void Update() //[PH]
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!abilities[0].onCooldown)
                    abilities[0].UseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!abilities[1].onCooldown)
                    abilities[1].UseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!abilities[2].onCooldown)
                    abilities[2].UseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!abilities[3].onCooldown)
                    abilities[3].UseAbility();
        }
        //[PH]
    }
}