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

    public Vector3 droneSpawnOffset = new Vector3(0, 1.5F, 0);

    private List<KamikazeDrone> spawnedDrones = new List<KamikazeDrone>();
    private int explodedDrones;
    private bool staggering;
    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        spawnedDrones.Clear();
        explodedDrones = 0;
        if (!staggering)
        {
            StartCoroutine(StaggerSpawns());
        }

        active = true;
    }

    private IEnumerator StaggerSpawns()
    {
        staggering = true;
        for (int i = 0; i < amountOfDrones; i++)
        {
            if(i == 0)
            {
                yield return new WaitForSeconds(0);
            }
            else
            {
                yield return new WaitForSeconds(droneDelay);
            }
            Vector3 horOffset = myPlayer.transform.right * droneSpawnOffset.x;
            Vector3 verOffset = myPlayer.transform.forward * droneSpawnOffset.z;
            Vector3 spawnPos = new Vector3(horOffset.x, droneSpawnOffset.y, verOffset.z);
            GameObject drone = Instantiate(dronePrefab, myPlayer.transform.position + spawnPos, myPlayer.transform.rotation);
            drone.GetComponent<KamikazeDrone>().Initialize(damage, aggroRadius, aoeRadius, myPlayer, this, droneSpeed);
            spawnedDrones.Add(drone.GetComponent<KamikazeDrone>());
        }

       
        staggering = false;
    }

    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        foreach (KamikazeDrone drone in spawnedDrones)
        {
            if(drone)
                drone.Explode();
        }

        StartCooldown();
    }

    public void CheckDrones()
    {
        explodedDrones++;
        if(explodedDrones >= amountOfDrones)
        {
            StopCoroutine(afterDurCoroutine);
            StartCooldown();
        }
        
    }
}
