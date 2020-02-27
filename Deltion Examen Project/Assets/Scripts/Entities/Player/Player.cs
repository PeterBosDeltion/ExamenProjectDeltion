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

        Initialize();
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

        if (Input.GetKeyDown("f"))
        {
            TakeDamage(10);
        }
        //[PH]
    }

    private void Initialize()
    {
        foreach (Ability ability in abilities)
        {
            Instantiate(ability, transform);
        }

        abilities.Clear();
        Ability[] abs = GetComponentsInChildren<Ability>();
        foreach (Ability a in abs)
        {
            a.myPlayer = this;
            abilities.Add(a);
        }

        hp = maxHp;
    }
    public void TakeDamage(float amount)
    {
        if (hp > 0 && tempHp <= 0)
        {
            hp -= amount;
        }
        else if (tempHp > 0)
        {
            tempHp -= amount;
        }

        if (tempHp < 0)
        {
            tempHp = 0;
        }
        if (hp < 0)
        {
            hp = 0;
        }
    }
}