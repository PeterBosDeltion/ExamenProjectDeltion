using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployableShield : MonoBehaviour
{
    private float myMaxHp;
    private float currentHp;
    private DeployableShieldAbility myAbility;
    public void Initialize(float hp, DeployableShieldAbility ability)
    {
        myMaxHp = hp;
        currentHp = myMaxHp;
    }

    public void OnDeath()
    {
        myAbility.ShieldDestroyed();
    }
}
