using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRifle : Weapon
{
    public float explosionAOE;
    public GameObject explosionParticle;
    public AudioClip explosionSfx;

    protected override void Shoot()
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
            bul.GetComponent<Bullet>().Initialize(myWeapon.damage, myWeapon.minFallOff, myWeapon.maxFallOff, bulletSpawn.transform.position, myPlayer, explosionAOE);
            bul.GetComponent<Bullet>().bloodParticle = explosionParticle;
            bul.GetComponent<Bullet>().impactSound = explosionSfx;
            rb.AddForce(bul.transform.forward * myWeapon.projectileVelocity);
            Destroy(bul, 2.0F);
            base.DrainAmmo();
        }
        if (magazineAmmo < 0 && !reloading)
        {
            magazineAmmo = 0;
        }

        if (magazineAmmo <= 0 && !reloading)
        {
            audioSource.clip = emptyMagazine;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

    }
}
