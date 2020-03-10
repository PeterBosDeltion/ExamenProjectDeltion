using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePulseAbility : Ability
{
    public float damage;
    public GameObject particlePrefab;
    private GameObject spawnedParticles;

    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        spawnedParticles = Instantiate(particlePrefab, myPlayer.transform);
        spawnedParticles.GetComponent<FlamePulse>().Initialize(damage, myPlayer);
        active = true;
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedParticles);
        spawnedParticles = null;
        StartCooldown();
    }
}
