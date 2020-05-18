using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployableShield : MonoBehaviour
{
  
    private DeployableShieldAbility myAbility;
    public void Initialize(DeployableShieldAbility ability)
    {
        myAbility = ability;
        myAbility.active = true;
        myAbility.afterDurCoroutine = StartCoroutine(myAbility.AfterDuration());
    }


}
