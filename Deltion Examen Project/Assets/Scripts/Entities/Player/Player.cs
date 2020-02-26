using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public HpEvent zeroHp;
    public HpEvent zeroTempHp;

    public List<Ability> abilities = new List<Ability>();

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
            abilities[0].UseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilities[1].UseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            abilities[2].UseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            abilities[3].UseAbility();
        }
        //[PH]
    }
}