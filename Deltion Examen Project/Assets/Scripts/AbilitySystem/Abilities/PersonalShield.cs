using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalShield : Ability
{
    public float tempHP;
    protected override void AbilityMechanic()
    {
        Debug.Log("Allow deploy");
    }
}
