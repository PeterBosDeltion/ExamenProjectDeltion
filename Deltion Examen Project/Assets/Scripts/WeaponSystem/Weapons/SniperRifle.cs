using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon
{

    public int maxPierceAmount = 5;
    public float pierceDamageDivision = 1.5F;

    private Entity closestEnemy;
    public float minEnemyDistance = 1F;
    private bool enemyTooClose;
    private void Update()
    {
        closestEnemy = GetClosestTarget();

        if(closestEnemy != null)
        {
            if (Vector3.Distance(myPlayer.transform.position, closestEnemy.transform.position) <= minEnemyDistance)
            {
                if (!enemyTooClose)
                    enemyTooClose = true;
            }
            else if(Vector3.Distance(myPlayer.transform.position, closestEnemy.transform.position) > minEnemyDistance)
            {
                if (enemyTooClose)
                    enemyTooClose = false;
            }
        }
       
        if (Input.GetMouseButtonUp(0))
        {
            ResetShotsFired();
        }
    }

    protected override void Shoot()
    {
        if (enemyTooClose)
        {
            if(Random.value > 0.5F)
            {
                myPlayer.SetUxText("No clear shot!");
            }
            else if(Random.value <= 0.5F)
            {
                myPlayer.SetUxText("Enemy too close!");
            }
            return;
        }
        if (gameObject.activeSelf && canShoot)
        {
            if (magazineAmmo > 0 && !reloading && canShoot)
            {
                shotsFired++;
                float offset = 0;
                if (shotsFired >= amountAccurateBullets)
                {
                    offset = Random.Range(myWeapon.minSpreadAngle, myWeapon.maxSpreadAngle);
                }
                GameObject bul = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                bul.transform.eulerAngles += new Vector3(0, offset, 0);
                Rigidbody rb = bul.GetComponent<Rigidbody>();
                bul.GetComponent<Bullet>().Initialize(myWeapon.damage, myWeapon.minFallOff, myWeapon.maxFallOff, bulletSpawn.transform.position, myPlayer, 0, true, maxPierceAmount, pierceDamageDivision);
                rb.GetComponent<Collider>().isTrigger = true;
                rb.AddForce(bul.transform.forward * myWeapon.projectileVelocity);

                Destroy(bul, 3.0F);
                DrainAmmo();
            }
            if (magazineAmmo < 0 && !reloading)
            {
                magazineAmmo = 0;
            }

            if (magazineAmmo <= 0 && !reloading)
            {
                if (muzzleFlashSystem.isPlaying)
                    muzzleFlashSystem.Stop();
                audioSource.clip = emptyMagazine;
                AudioClipManager.instance.PlayClipOneShotWithSource(myPlayer.mySource, AudioClipManager.instance.GetRandomNoAmmoVL(myPlayer));
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    private Entity GetClosestTarget()
    {
        if (EntityManager.instance.AllEnemys.Count != 0)
        {
            Entity closestEntity = null;
            float rangeToClosest = Mathf.Infinity;
            foreach (Entity entity in EntityManager.instance.AllEnemys)
            {
                if(entity != null)
                {
                    float newDistance = Vector3.Distance(transform.position, entity.transform.position);
                    if (rangeToClosest > newDistance && entity.enabled && !entity.death)
                    {
                        rangeToClosest = newDistance;
                        closestEntity = entity;
                    }
                }
              
            }

            return closestEntity;
        }
        else
        {
            return null;
        }
    }
}
