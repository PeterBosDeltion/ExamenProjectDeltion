using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>();

    public float hp = 1000;
    public float tempHp;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilities[0].UseAbility();
        }
    }

    public void TakeDamage(float amount)
    {
        if(hp > 0)
        {
            hp -= amount;
        }
        else if(tempHp > 0)
        {
            tempHp -= amount;
        }

        if(tempHp < 0)
        {
            tempHp = 0;
        }
        if(hp < 0)
        {
            hp = 0;
        }
    }
}
