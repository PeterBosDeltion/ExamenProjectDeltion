using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBombardmentUltimateAbility : Ability
{
    public GameObject shipShadowPrefab;
    public GameObject targetingSprite;
    public Vector3 offsetFromPlayer = new Vector3();
    private GameObject spawnedShip;

    public float targetRadius;
    public float damageRadius;
    public float damage;

    private bool canSpawn;
    private bool waiting;
    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        spawnedShip = Instantiate(shipShadowPrefab, myPlayer.transform.position + offsetFromPlayer, Quaternion.identity);
        spawnedShip.transform.localEulerAngles += new Vector3(0, -90, 0);
        canSpawn = true;
    
        active = true;
    }

    private Vector3 FindPoint()
    {
        Vector3 point = new Vector2(myPlayer.transform.position.x, myPlayer.transform.position.z) + Random.insideUnitCircle * targetRadius;
        point.z = point.y;
        point.y = 0.1F;
        return point;
    }

    private void Update()
    {
        if (canSpawn)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject spr = Instantiate(targetingSprite, FindPoint(), Quaternion.identity);
                spr.GetComponent<OrbitalBombardmentUltimate>().Initialize(damageRadius, damage, myPlayer);
               
                    canSpawn = false;
                    if (!waiting)
                    {
                        StartCoroutine(RefireTime());
                    }
               
            }

            
        }
    }

    private IEnumerator RefireTime()
    {
        waiting = true;
        yield return new WaitForSeconds(6);
        canSpawn = true;
        waiting = false;
    }

    protected override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedShip);
        spawnedShip = null;
        StartCooldown();
    }
}
