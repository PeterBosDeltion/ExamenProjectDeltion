using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalShield : Ability
{
    public float tempHP = 500;
    public GameObject shieldEffectPrefab;
    private GameObject shieldObject;
    protected override void AbilityMechanic()
    {
        myPlayer.tempHp = tempHP;
        shieldObject = Instantiate(shieldEffectPrefab);
        shieldObject.transform.SetParent(myPlayer.transform);
        shieldObject.transform.localPosition = Vector3.zero;
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(shieldObject);
        myPlayer.tempHp = 0;
    }
}
