using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponScriptable myWeapon; //Probably going te be initialized by IDmanager later
    public GameObject bullet; //Might need to be changed if guns fire different projectiles
    public GameObject bulletSpawn;
    protected float totalAmmo;
    protected float magazineAmmo;
    private AudioSource audioSource;

    public AudioClip gunShot;
    public AudioClip emptyMagazine;
    public AudioClip reload;

    private bool reloading;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        totalAmmo = myWeapon.totalAmmo;
        magazineAmmo = totalAmmo;
        audioSource = GetComponent<AudioSource>();
    }
    protected virtual void Shoot()
    {
        if(magazineAmmo > 0 && !reloading)
        {
            GameObject bul = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            Rigidbody rb = bul.GetComponent<Rigidbody>();
            bul.GetComponent<Bullet>().Initialize(myWeapon.damage, myWeapon.minFallOff, myWeapon.maxFallOff, bulletSpawn.transform.position);
            rb.AddForce(bul.transform.forward * myWeapon.projectileVelocity);
            Destroy(bul, 3.0F);
            DrainAmmo();
        }
        else if(magazineAmmo < 0 && !reloading)
        {
            magazineAmmo = 0;
        }

        if(magazineAmmo <= 0)
        {
            audioSource.clip = emptyMagazine;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void DrainAmmo()
    {
        magazineAmmo -= myWeapon.ammoDrain;
        audioSource.clip = gunShot;
        audioSource.PlayOneShot(gunShot);
    }

    protected virtual void Reload()
    {
        if (!reloading)
        {
            StartCoroutine(ReloadInSeconds(myWeapon.reloadSpeed));
            audioSource.clip = reload;
            audioSource.Play();
        }
    }

    private IEnumerator ReloadInSeconds(float seconds)
    {
        reloading = true;
        Debug.Log("Start reload");
        yield return new WaitForSeconds(seconds);
        magazineAmmo = totalAmmo;
        Debug.Log("Reloaded");
        reloading = false;
    }
}
