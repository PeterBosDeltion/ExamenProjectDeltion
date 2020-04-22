using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLaserRifle : Weapon
{
    public LineRenderer lineRend;

  
    protected override void Shoot()
    {
        if (gameObject.activeSelf)
        {
            if (magazineAmmo > 0 && !reloading && canShoot)
            {
                lineRend.positionCount = 2;
                lineRend.SetPosition(0, bulletSpawn.transform.position);
                lineRend.SetPosition(1, bulletSpawn.transform.forward * 30 + bulletSpawn.transform.position);


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

    protected override void ResetShotsFired()
    {
        lineRend.positionCount = 0;
    }
}
