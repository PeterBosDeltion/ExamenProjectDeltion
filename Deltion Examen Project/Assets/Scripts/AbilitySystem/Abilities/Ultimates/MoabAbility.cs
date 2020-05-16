using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoabAbility : Ability
{
    private Vector3 centerPoint;
    public GameObject bombPrefab;

    public float damageRadius;
    public float damage;
    public float aoeDamage;
    public float aoeRadius;
    public float bombDownForce = 50;

    private GameObject spawnedBomb;
    [HideInInspector]
    public GameObject spawnedAOE;

    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        centerPoint = (Vector3)mPos;
        spawnedBomb = Instantiate(bombPrefab, centerPoint + deployableOffset, Quaternion.identity);
        spawnedBomb.GetComponent<Rigidbody>().AddForce(Vector3.down * bombDownForce );

        spawnedBomb.GetComponent<Moab>().Initialize(damageRadius, damage, myPlayer, this);

        active = true;

    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedAOE);
        StartCooldown();

    }
}
