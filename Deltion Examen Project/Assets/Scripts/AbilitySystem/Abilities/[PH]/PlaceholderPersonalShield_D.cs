using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderPersonalShield_D : Ability
{
    public float tempHP = 500;
    public GameObject shieldEffectPrefab;
    private GameObject shieldObject;
    private List<GameObject> activeShields = new List<GameObject>();
    public Vector3 spawnOffset;

    public void Start()
    {
        myPlayer.zeroTempHp += ResetCoroutine;
    }

    protected override void AbilityMechanic()
    {
        if (activeShields.Count > 0)
        {
            for (int i = 0; i < activeShields.Count - 1; i++)
            {
                Destroy(activeShields[i]);
            }

            activeShields.Clear();
        }
        myPlayer.Heal(0, tempHP);
        shieldObject = Instantiate(shieldEffectPrefab);
        shieldObject.transform.SetParent(myPlayer.transform);
        shieldObject.transform.localPosition = Vector3.zero + spawnOffset;
        activeShields.Add(shieldObject);
    }

    protected void ResetCoroutine(Entity Attacker)
    {
        StopCoroutine(afterDurCoroutine);
        activeShields.Clear();
        Destroy(shieldObject);

        //kwam er bij met merge conflict. Is dit nodig?
        StartCooldown();
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        activeShields.Clear();
        Destroy(shieldObject);
        myPlayer.RemoveTempHP();

        //kwam er bij met merge conflict. Is dit nodig?
        StartCooldown();
    }
}
