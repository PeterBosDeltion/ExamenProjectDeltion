using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalDroneAbility : Ability
{
    public float healPool;
    public float reloadSpeed;
    public float healRate = 1;
    public GameObject dronePrefab;
    private GameObject spawnedDrone;
    protected override void AbilityMechanic(Vector3? mPos)
    {
        Vector3 spawnPos = myPlayer.transform.position + new Vector3(0, 0.5F, 0);
        spawnedDrone = Instantiate(dronePrefab, spawnPos, Quaternion.identity);
        spawnedDrone.GetComponent<MedicalDrone>().Initialize(healPool, myPlayer, reloadSpeed, healRate);

        active = true;
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedDrone);
        spawnedDrone = null;
        StartCooldown();
    }
}
