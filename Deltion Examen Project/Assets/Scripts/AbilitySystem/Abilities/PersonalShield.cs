using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalShield : Ability
{
    public float tempHP = 500;
    public GameObject shieldEffectPrefab;
    private GameObject shieldObject;
    private List<GameObject> activeShields = new List<GameObject>();
    public Vector3 spawnOffset;
    private bool activeShield;
    protected override void AbilityMechanic()
    {
        if(activeShields.Count > 0)
        {
            for (int i = 0; i < activeShields.Count -1; i++)
            {
                Destroy(activeShields[i]);
            }

            activeShields.Clear();
        }
        myPlayer.tempHp = tempHP;
        shieldObject = Instantiate(shieldEffectPrefab);
        shieldObject.transform.SetParent(myPlayer.transform);
        shieldObject.transform.localPosition = Vector3.zero  + spawnOffset;
        activeShields.Add(shieldObject);
        activeShield = true;
    }

    private void Update()
    {
        if (activeShield)
        {
            if(myPlayer.tempHp <= 0)
            {
                StopCoroutine(afterDurCoroutine);
                activeShields.Clear();
                Destroy(shieldObject);
                myPlayer.tempHp = 0;
                StartCooldown();
                activeShield = false;
            }
        }
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        activeShields.Clear();
        Destroy(shieldObject);
        myPlayer.tempHp = 0;
        StartCooldown();
        activeShield = false;
    }
}
