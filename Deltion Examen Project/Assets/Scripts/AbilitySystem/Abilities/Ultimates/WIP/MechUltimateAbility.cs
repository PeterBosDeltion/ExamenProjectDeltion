using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUltimateAbility : Ability
{
    public GameObject mechPrefab;
    private GameObject spawnedMech;

    public float mechHp;
    public float mechDamage;

    private bool dead;


    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        spawnedMech = Instantiate(mechPrefab, myPlayer.transform.position, myPlayer.transform.rotation);
        spawnedMech.GetComponent<MechUltimate>().Initialize(mechHp, mechDamage, this);
        myPlayer.GetComponent<PlayerController>().DisablePlayer();
        dead = false;
        active = true;
    }

    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedMech);
        spawnedMech = null;
        myPlayer.GetComponent<PlayerController>().EnablePlayer();
        StartCooldown();
    }

    public void MechDestroyed()
    {
        //if (!dead)
        //{
            dead = true;
            StopCoroutine(afterDurCoroutine);
            Destroy(spawnedMech);
            spawnedMech = null;
            myPlayer.GetComponent<PlayerController>().EnablePlayer();
            StartCooldown();
        //}
       
    }

    private void FixedUpdate()
    {
        if (spawnedMech)
        {
            myPlayer.transform.position = spawnedMech.transform.position;
            myPlayer.transform.rotation = spawnedMech.transform.rotation;
        }
    }
}
