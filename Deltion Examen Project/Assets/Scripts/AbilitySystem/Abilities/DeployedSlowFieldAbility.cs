using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedSlowFieldAbility : Ability
{
    public GameObject slowFieldPrefab;
    public float slowPercentage = 10;
    public float fieldRadius = 2.5F;
    private GameObject spawnedField;
    protected override void AbilityMechanic(Vector3? mPos, Quaternion? deployRotation)
    {
        spawnedField = Instantiate(slowFieldPrefab, (Vector3)mPos, (Quaternion)deployRotation);
        spawnedField.GetComponent<DeployedSlowField>().Initialize(slowPercentage, fieldRadius, this);
    }

    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        spawnedField.GetComponent<DeployedSlowField>().ResetValues();
        Destroy(spawnedField);
        spawnedField = null;
        StartCooldown();
    }
}
