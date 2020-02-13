using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>();

    public float hp = 1000;
    public float tempHp;
    // Update is called once per frame

    private void Start()
    {
        Initialize();
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

    private void Initialize()
    {
        //for (int i = 0; i < abilities.Count -1; i++)
        //{
        //    InputManager.Instance.abilityEvent += abilities[i].UseAbility;
        //}
    }
    public void TakeDamage(float amount)
    {
        if (hp > 0)
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