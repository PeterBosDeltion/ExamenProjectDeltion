using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponScriptable myWeapon; //Probably going te be initialized by IDmanager later
    public GameObject bullet; //Might need to be changed if guns fire different projectiles
    public GameObject bulletSpawn;
    public float totalAmmo;
    public float magazineAmmo;
    public bool canShoot = true;
    protected AudioSource audioSource;

    public AudioClip gunShot;
    public AudioClip emptyMagazine;
    public AudioClip reload;

    protected bool reloading;
    protected int shotsFired;
    public int amountAccurateBullets;

    public Player myPlayer;
    public bool tutorialInit;

    public Vector3 handPosition;
    public Vector3 handRotation;

    private bool waiting;
    private Coroutine LimitFirerateCoroutine;
    private Coroutine ReloadCoroutine;

    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private void Start()
    {
        tutorialInit = false;
        if (!waiting)
            StartCoroutine(WaitForPlayerInit());
    }

    private IEnumerator WaitForPlayerInit()
    {
        waiting = true;
        yield return new WaitUntil(() => myPlayer.GetComponent<PlayerController>().playerInitialized);
        Initialize();
        waiting = false;
    }

    public void Initialize()
    {
        if (myPlayer.GetComponent<PlayerController>().inTutorial && !tutorialInit)
        {
            return;
        }
        myPlayer = GetComponentInParent<Player>();
        totalAmmo = myWeapon.totalAmmo;
        magazineAmmo = totalAmmo;
        audioSource = GetComponent<AudioSource>();
        canShoot = true;
        switch (myWeapon.myFireType)
        {
            case WeaponScriptable.FireType.Auto:
                InputManager.leftMouseButtonHoldEvent += Shoot;
                break;
            case WeaponScriptable.FireType.Semi:
                InputManager.leftMouseButtonEvent += Shoot;
                break;
            case WeaponScriptable.FireType.Bolt:
                InputManager.leftMouseButtonEvent += Shoot;
                break;
        }
        InputManager.leftMouseButtonUpEvent += ResetShotsFired;
        InputManager.reloadEvent += Reload;
    }

    private void OnDestroy()
    {
        InputManager.leftMouseButtonEvent -= Shoot;
        InputManager.leftMouseButtonHoldEvent -= Shoot;
        InputManager.leftMouseButtonUpEvent -= ResetShotsFired;
        InputManager.reloadEvent -= Reload;
    }

    protected virtual void Shoot()
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
                bul.GetComponent<Bullet>().Initialize(myWeapon.damage, myWeapon.minFallOff, myWeapon.maxFallOff, bulletSpawn.transform.position, myPlayer);
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

    public void ResetValues()
    {
        ResetShotsFired();
        canShoot = true;
        reloading = false;
    }

    protected void DrainAmmo()
    {
        if (gameObject.activeSelf)
        {
            magazineAmmo -= myWeapon.ammoDrain;
            audioSource.clip = gunShot;
            audioSource.PlayOneShot(gunShot);
            if (canShoot)
            {
                float refireTime = 60 / myWeapon.firerate;
                LimitFirerateCoroutine = StartCoroutine(LimitFireRate(refireTime));
                if (!activeCoroutines.Contains(LimitFirerateCoroutine))
                    activeCoroutines.Add(LimitFirerateCoroutine);
            }
        }
       
    }

    protected virtual void Reload()
    {
        if (gameObject.activeSelf)
        {
            if (!reloading && magazineAmmo < totalAmmo)
            {
                ReloadCoroutine = StartCoroutine(ReloadInSeconds(myWeapon.reloadSpeed));
                if (!activeCoroutines.Contains(ReloadCoroutine))
                    activeCoroutines.Add(ReloadCoroutine);
                audioSource.clip = reload;
                audioSource.Play();
                AudioClipManager.instance.HardResetSourcePlayable(myPlayer.mySource);
                AudioClipManager.instance.PlayClipOneShotWithSource(myPlayer.mySource, AudioClipManager.instance.GetRandomReloadVL(myPlayer));

            }
        }
      
    }

    protected virtual void ResetShotsFired()
    {
        shotsFired = 0;
    }
    private IEnumerator LimitFireRate(float refireTime)
    {
        canShoot = false;
        yield return new WaitForSeconds(refireTime);
        if (activeCoroutines.Contains(LimitFirerateCoroutine))
            activeCoroutines.Remove(LimitFirerateCoroutine);
        canShoot = true;
    }

    private IEnumerator ReloadInSeconds(float seconds)
    {
        reloading = true;
        yield return new WaitForSeconds(seconds);
        shotsFired = 0;
        magazineAmmo = totalAmmo;
        if (activeCoroutines.Contains(ReloadCoroutine))
            activeCoroutines.Remove(ReloadCoroutine);
        reloading = false;
    }

    public void StopCoroutines()
    {
        foreach (Coroutine c in activeCoroutines)
        {
            StopCoroutine(c);
        }
    }
}
