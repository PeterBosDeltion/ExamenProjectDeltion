using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentryTurret : Entity
{
    public GameObject bulletPrefab;

    public GameObject rotatingBody;
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject bulletSpawnOne;
    public GameObject bulletSpawnTwo;
    private float myBulletForce;
    private float myRange;
    private float myAggroRadius;
    private float myDamage;
    public float RotSpeed;

    private Entity target;
    private Player myPlayer;

    private float myMaxAmmo;
    //private float currentAmmo;
    //private bool reloading;
    //private float myReloadTime;
    private bool canShoot;
    private float myFirerate;
    private float armSpeed;
    public Image healthBar;

    public Color flashFrom;
    public Color flashTo;

    public Image ammoImg;
    private bool flashing;
    private SentryTurretAbility myAbility;

    public void Initialize(float range, float damage, float aggroRadius, Player player, float bulletForce, float maxAmmo, float reloadTime, float firerate, SentryTurretAbility ability)
    {
        myRange = range;
        myDamage = damage;
        myAggroRadius = aggroRadius;
        myPlayer = player;
        myBulletForce = bulletForce;
        myMaxAmmo = maxAmmo;
        //currentAmmo = myMaxAmmo;
        //myReloadTime = reloadTime;
        myFirerate = firerate;
        hp = maxHp;
        canShoot = true;
        myAbility = ability;
        armSpeed = RotSpeed * 40000;
        ammoImg.gameObject.SetActive(false);
        EntityManager.instance.AddPlayerOrAbility(this);
    }

    private void FixedUpdate()
    {
        Rotate();

        if (target)
        {
            if (target.GetComponent<Entity>().GetHp() > 0)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= myRange) //&& currentAmmo > 0
                {
                    leftArm.transform.Rotate(Vector3.up * armSpeed * Time.deltaTime);
                    rightArm.transform.Rotate(Vector3.up * armSpeed * Time.deltaTime);
                }
            }
        }
                   
    }

    private void Update()
    {
        FindTarget();
        Shoot();

        healthBar.fillAmount = hp / maxHp;
    }

    private void Rotate()
    {
        if (!target)
        {
            rotatingBody.transform.Rotate(Vector3.up * 45 * Time.deltaTime);
        }
        else
        {
            Vector3 lookPos = target.transform.position - rotatingBody.transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            rotatingBody.transform.rotation = Quaternion.Slerp(rotatingBody.transform.rotation, rotation, Time.deltaTime * RotSpeed);
        }
    }

    private void FindTarget()
    {
        //if (!target)
        //{
        //    Collider[] overlaps = Physics.OverlapSphere(transform.position, myAggroRadius);
        //    if (overlaps.Length > 0)
        //    {
        //        foreach (var collider in overlaps)
        //        {
        //            if (collider.transform.GetComponent<Entity>() && !collider.transform.GetComponent<Player>() && collider.transform != transform)
        //            {
        //                target = collider.transform;
        //            }
        //        }
        //    }
        //}

        if (EntityManager.instance.AllEnemys.Count != 0)
            target = GetClosestTarget();

      
    }

    private Entity GetClosestTarget()
    {
        if (EntityManager.instance.AllEnemys.Count != 0)
        {
            Entity closestEntity = null;
            float rangeToClosest = Mathf.Infinity;
            foreach (Entity entity in EntityManager.instance.AllEnemys)
            {
                float newDistance = Vector3.Distance(transform.position, entity.transform.position);
                if (rangeToClosest > newDistance && entity.enabled && !entity.death)
                {
                    rangeToClosest = newDistance;
                    closestEntity = entity;
                }
            }

            return closestEntity;
        }
        else
        {
            return null;
        }
    }

    private void Shoot()
    {
        if (target)
        {
            if(target.GetComponent<Entity>().GetHp() > 0)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= myRange)
                {
                    if (canShoot) //currentAmmo > 0 && !reloading && 
                    {
                        GameObject bulletOne = Instantiate(bulletPrefab, bulletSpawnOne.transform.position, bulletSpawnOne.transform.rotation);
                        GameObject bulletTwo = Instantiate(bulletPrefab, bulletSpawnTwo.transform.position, bulletSpawnTwo.transform.rotation);

                        Rigidbody rbOne = bulletOne.GetComponent<Rigidbody>();
                        Rigidbody rbTwo = bulletTwo.GetComponent<Rigidbody>();

                        Bullet bOne = bulletOne.GetComponent<Bullet>();
                        Bullet bTwo = bulletTwo.GetComponent<Bullet>();

                        bOne.Initialize(myDamage, 20, 60, bulletSpawnOne.transform.position, this);
                        bTwo.Initialize(myDamage, 20, 60, bulletSpawnTwo.transform.position, this);

                        rbOne.AddForce(bulletSpawnOne.transform.forward * myBulletForce);
                        rbTwo.AddForce(bulletSpawnTwo.transform.forward * myBulletForce);
                        //currentAmmo -= 2;

                        if (canShoot)
                        {
                            float refireTime = 60 / myFirerate;
                            StartCoroutine(LimitFireRate(refireTime));
                        }

                       
                    }
                    //else if (currentAmmo <= 0)
                    //{
                    //    if (!reloading)
                    //    {
                    //        StartCoroutine(Reload());
                    //    }
                    //}

                }
                else
                {
                    target = null;
                }
            }
            else
            {
                target = null;
            }
        }
    }

    //private IEnumerator Reload()
    //{
    //    reloading = true;
    //    if (!flashing)
    //    {
    //        ammoImg.gameObject.SetActive(true);
    //        ammoImg.color = flashFrom;
    //        InvokeRepeating("FlashAmmo", 0, 1);
    //    }
    //    yield return new WaitForSeconds(myReloadTime);
    //    CancelInvoke("FlashAmmo");
    //    ammoImg.gameObject.SetActive(false);
    //    flashing = false;
    //    currentAmmo = myMaxAmmo;
    //    reloading = false;
    //}

    private IEnumerator LimitFireRate(float refireTime)
    {
        canShoot = false;
        yield return new WaitForSeconds(refireTime);
        canShoot = true;
    }

    private void FlashAmmo()
    {
        flashing = true;
        ammoImg.color = (ammoImg.color == flashFrom) ? ammoImg.color = flashTo : ammoImg.color = flashFrom;
    }

    protected override void Death()
    {
        myAbility.TurretDestroyed();
    }
}
