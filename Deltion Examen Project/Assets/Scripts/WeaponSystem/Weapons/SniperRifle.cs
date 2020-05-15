using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon
{
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ResetShotsFired();
        }
    }

    protected override void Shoot()
    {
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
                bul.GetComponent<Bullet>().Initialize(myWeapon.damage, myWeapon.minFallOff, myWeapon.maxFallOff, bulletSpawn.transform.position, myPlayer, 0, true);
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
                audioSource.clip = emptyMagazine;
                AudioClipManager.instance.PlayClipOneShotWithSource(myPlayer.mySource, AudioClipManager.instance.GetRandomNoAmmoVL(myPlayer));
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }
}
