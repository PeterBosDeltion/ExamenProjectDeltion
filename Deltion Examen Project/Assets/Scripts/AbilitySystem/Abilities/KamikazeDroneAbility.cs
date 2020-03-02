using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeDroneAbility : Ability
{
    public GameObject dronePrefab;

    public int amountOfDrones;
    public float damage;
    public float aoeRadius;
    public float droneSpeed;
    public float range;
    public float aggroRadius;
    public float droneDelay;

    public float droneSpawnY = 2.5F;
    public float droneSpacingX = 0.5F;
    public float droneSpacingZ = 0.25F;

    private List<KamikazeDrone> spawnedDrones = new List<KamikazeDrone>();
    private int explodedDrones;
    protected override void AbilityMechanic(Vector3? mPos = null)
    {
        spawnedDrones.Clear();
        explodedDrones = 0;
        float newDelay = 0;
        Vector3 spawnPos = myPlayer.transform.position + new Vector3(0, droneSpawnY, 0);
        for (int i = 0; i < amountOfDrones; i++)
        {
            if(i != 0)
            {
                newDelay = droneDelay + droneDelay * i;
            }
            float roundedHalf = Mathf.RoundToInt(amountOfDrones / 2);
            if (i < amountOfDrones / 2 && i != 0)
            {
                spawnPos += new Vector3(droneSpacingX, 0, -droneSpacingZ);
            }
            else if(i == roundedHalf)
            {
                spawnPos = myPlayer.transform.position + new Vector3(-droneSpacingX, droneSpawnY, -droneSpacingZ);
            }
            else if(i > roundedHalf)
            {
                spawnPos += new Vector3(-droneSpacingX, 0, -droneSpacingZ);
            }

            GameObject drone = Instantiate(dronePrefab, spawnPos, Quaternion.identity);
            drone.GetComponent<KamikazeDrone>().Initialize(range, damage, aggroRadius, aoeRadius, myPlayer, this, newDelay, droneSpeed);
            spawnedDrones.Add(drone.GetComponent<KamikazeDrone>());
        }

        active = true;
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        foreach (KamikazeDrone drone in spawnedDrones)
        {
            drone.Explode();
        }

        StartCooldown();
    }

    public void CheckDrones()
    {
        explodedDrones++;
        if(explodedDrones >= spawnedDrones.Count)
        {
            StopCoroutine(afterDurCoroutine);
            StartCooldown();
        }
    }
}
