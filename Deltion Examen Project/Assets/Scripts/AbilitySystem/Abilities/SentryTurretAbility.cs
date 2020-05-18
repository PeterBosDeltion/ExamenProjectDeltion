using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTurretAbility : Ability
{
    public GameObject turretPrefab;
    public float range;
    public float aggroRadius;
    public float damage;
    public float rotSpeed;
    public float bulletforce;
    public float maxAmmo;
    public float reloadTime;
    public float fireRate;

    private GameObject spawnedTurret;
    private SentryTurret turret;
    protected override void AbilityMechanic(Vector3? mPos, Quaternion? deployRotation)
    {
            spawnedTurret = Instantiate(turretPrefab, (Vector3)mPos, (Quaternion)deployRotation);
            turret = spawnedTurret.GetComponent<SentryTurret>();
            turret.Initialize(range, damage, aggroRadius, myPlayer, bulletforce, maxAmmo, reloadTime, fireRate, this);
    }

    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        turret.TriggerDeathEvents();
        Destroy(spawnedTurret);
        spawnedTurret = null;
        StartCooldown();
    }

    public void TurretDestroyed()
    {
        StopCoroutine(afterDurCoroutine);
        Destroy(spawnedTurret);
        spawnedTurret = null;
        AudioClipManager.instance.PlayClipOneShotWithSource(myPlayer.mySource, AudioClipManager.instance.clips.voiceDeployableDestroyed);
        myPlayer.SetUxText("Deployable destroyed!");
        StartCooldown();
    }
}
