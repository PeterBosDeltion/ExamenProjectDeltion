using cakeslice;
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
    public PlayerController myPlayerController;
    public bool tutorialInit;

    public Vector3 handPosition;
    public Vector3 handRotation;

    private bool waiting;
    private Coroutine LimitFirerateCoroutine;
    private Coroutine ReloadCoroutine;

    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private LineRenderer laserTarget;
    private GameObject laserEndPos;
    public bool bolt;
    private bool wait;
    private bool waitFlash;
    private Coroutine flashRoutine;
    public ParticleSystem muzzleFlashSystem;
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
        myPlayer = GetComponentInParent<Player>();
        laserTarget = GetComponentInChildren<LineRenderer>();
        muzzleFlashSystem = GetComponentInChildren<ParticleSystem>();
        if (laserTarget)
        {
            laserTarget.positionCount = 0;
        }
        else
        {
            Debug.Log("Plase add laser visuals to " + gameObject.name);
        }

        if (myPlayerController.inTutorial && !tutorialInit)
        {
            return;
        }
        totalAmmo = myWeapon.totalAmmo;
        magazineAmmo = totalAmmo;
        audioSource = GetComponent<AudioSource>();
        canShoot = true;
        switch (myWeapon.myFireType)
        {
            case WeaponScriptable.FireType.Auto:
                myPlayerController.myInputManager.leftMouseButtonHoldEvent += Shoot;
                break;
            case WeaponScriptable.FireType.Semi:
                 myPlayerController.myInputManager.leftMouseButtonEvent += Shoot;
                break;
            case WeaponScriptable.FireType.Bolt:
                 myPlayerController.myInputManager.leftMouseButtonEvent += Shoot;
                break;
        }
         myPlayerController.myInputManager.leftMouseButtonUpEvent += ResetShotsFired;
         myPlayerController.myInputManager.leftMouseButtonUpEvent += ResetFlash;
         myPlayerController.myInputManager.reloadEvent += Reload;

        //cakeslice.Outline[] lines = GetComponentsInChildren<Outline>(); Doesn't look great on most weapons, but may be considered later
        //foreach (Outline l in lines)
        //{
        //    l.color = myPlayerController.playerNumber;
        //}
    }

    public void SetLaser(GameObject target)
    {
            laserEndPos = target;
            if (laserTarget.positionCount != 2)
                laserTarget.positionCount = 2;

            if(laserTarget.positionCount != 0)
            {
                laserTarget.SetPosition(0, bulletSpawn.transform.position);
                laserTarget.SetPosition(1, laserEndPos.transform.position);
            }
           
    }

    public void CancelLaser()
    {
        laserTarget.positionCount = 0;
        laserEndPos = null;
    }

    private void ResetFlash()
    {
        if(gameObject.activeSelf && !waitFlash)
            flashRoutine = StartCoroutine(ResetFlashCoroutine());
    }

    private IEnumerator ResetFlashCoroutine()
    {
        waitFlash = true;
        yield return new WaitForSeconds(.15F);
        muzzleFlashSystem.Stop();
        waitFlash = false;
    }

    private void OnDestroy()
    {
         myPlayerController.myInputManager.leftMouseButtonEvent -= Shoot;
         myPlayerController.myInputManager.leftMouseButtonHoldEvent -= Shoot;
         myPlayerController.myInputManager.leftMouseButtonUpEvent -= ResetShotsFired;
         myPlayerController.myInputManager.reloadEvent -= Reload;
        myPlayerController.myInputManager.leftMouseButtonUpEvent -= ResetFlash;

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

            if (myWeapon.myFireType == WeaponScriptable.FireType.Bolt)
            {
                bolt = true;
                if (!wait)
                    StartCoroutine(ResetBolt());
            }
            magazineAmmo -= myWeapon.ammoDrain;
            audioSource.clip = gunShot;
            audioSource.PlayOneShot(gunShot);
            if (!muzzleFlashSystem.isPlaying)
            {
                if(flashRoutine != null)
                    StopCoroutine(flashRoutine);
                waitFlash = false;
                muzzleFlashSystem.Play();
                ResetFlash();
            }


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
            if (!reloading && magazineAmmo < totalAmmo && !bolt)
            {
                muzzleFlashSystem.Stop();
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

    private IEnumerator ResetBolt()
    {
        wait = true;
        myPlayerController.myInputManager.reloadEvent.Invoke();
        yield return new WaitForSeconds(myWeapon.boltTime);
        bolt = false;
        canShoot = true;
        wait = false;
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
        if(!bolt)
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
