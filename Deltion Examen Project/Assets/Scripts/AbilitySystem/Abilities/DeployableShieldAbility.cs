using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployableShieldAbility : Ability
{
    public GameObject deployableShieldPrefab;
    public float shieldHp;
    private GameObject spawnedShield;
    protected override void AbilityMechanic(Vector3? mPos, Quaternion? deployRotation)
    {
        spawnedShield = Instantiate(deployableShieldPrefab, (Vector3)mPos, (Quaternion)deployRotation);
        spawnedShield.GetComponent<DeployableShield>().Initialize(shieldHp, this);

        active = true;
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedShield);
        spawnedShield = null;
        Debug.Log("after");
        StartCooldown();
    }

    public void ShieldDestroyed()
    {
        StopCoroutine(afterDurCoroutine);
        Destroy(spawnedShield);
        spawnedShield = null;
        StartCooldown();
    }
}
