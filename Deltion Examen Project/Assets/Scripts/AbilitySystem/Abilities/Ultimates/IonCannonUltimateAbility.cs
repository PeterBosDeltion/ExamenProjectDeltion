using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonCannonUltimateAbility : Ability
{
    private Vector3 centerPoint;
    public GameObject ionCannonPrefab;
    private GameObject spawnedCannon;

    public float damage;
    public float damageTickInterval;

    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        centerPoint = (Vector3)mPos;
        spawnedCannon = Instantiate(ionCannonPrefab, centerPoint + deployableOffset, Quaternion.identity);
        spawnedCannon.GetComponent<IonCannon>().Initialize(damage, myPlayer, damageTickInterval);
        active = true;

    }

    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedCannon);
        StartCooldown();

    }
}
