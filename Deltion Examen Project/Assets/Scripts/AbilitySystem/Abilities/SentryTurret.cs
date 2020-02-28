using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTurret : Entity
{
    public GameObject bulletPrefab;

    public GameObject rotatingBody;
    public GameObject bulletSpawnOne;
    public GameObject bulletSpawnTwo;
    private float myBulletForce;
    private float myRange;
    private float myAggroRadius;
    private float myDamage;
    public float RotSpeed;

    private Transform target;
    private Player myPlayer;

    private float myMaxAmmo;
    private float currentAmmo;
    private bool reloading;
    private float myReloadTime;
    private bool canShoot;
    private float myFirerate;

    public void Initialize(float range, float damage, float aggroRadius, Player player, float bulletForce, float maxAmmo, float reloadTime, float firerate)
    {
        myRange = range;
        myDamage = damage;
        myAggroRadius = aggroRadius;
        myPlayer = player;
        myBulletForce = bulletForce;
        myMaxAmmo = maxAmmo;
        currentAmmo = myMaxAmmo;
        myReloadTime = reloadTime;
        myFirerate = firerate;

        canShoot = true;

    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Update()
    {
        FindTarget();
        Shoot();
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
        if (!target)
        {
            Collider[] overlaps = Physics.OverlapSphere(transform.position, myAggroRadius);
            if (overlaps.Length > 0)
            {
                foreach (var collider in overlaps)
                {
                    if (collider.transform.GetComponent<Enemy>())
                    {
                        target = collider.transform;
                    }
                }
            }
        }
      
    }

    private void Shoot()
    {
        if (target)
        {
            if(Vector3.Distance(transform.position, target.position) <= myRange)
            {
                if(currentAmmo > 0 && !reloading && canShoot)
                {
                    GameObject bulletOne = Instantiate(bulletPrefab, bulletSpawnOne.transform.position, bulletSpawnOne.transform.rotation);
                    GameObject bulletTwo = Instantiate(bulletPrefab, bulletSpawnTwo.transform.position, bulletSpawnTwo.transform.rotation);

                    Rigidbody rbOne = bulletOne.GetComponent<Rigidbody>();
                    Rigidbody rbTwo = bulletTwo.GetComponent<Rigidbody>();

                    Bullet bOne = bulletOne.GetComponent<Bullet>();
                    Bullet bTwo = bulletTwo.GetComponent<Bullet>();

                    bOne.Initialize(myDamage, 20, 60, bulletSpawnOne.transform.position, myPlayer);
                    bTwo.Initialize(myDamage, 20, 60, bulletSpawnTwo.transform.position, myPlayer);

                    rbOne.AddForce(bulletSpawnOne.transform.forward * myBulletForce);
                    rbTwo.AddForce(bulletSpawnTwo.transform.forward * myBulletForce);
                    currentAmmo -= 2;

                    if (canShoot)
                    {
                        float refireTime = 60 / myFirerate;
                        StartCoroutine(LimitFireRate(refireTime));
                    }
                }
                else if(currentAmmo <= 0)
                {
                    if (!reloading)
                    {
                        StartCoroutine(Reload());
                    }
                }
               
            }
            else
            {
                target = null;
            }
        }
    }

    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(myReloadTime);
        currentAmmo = myMaxAmmo;
        reloading = false;
    }

    private IEnumerator LimitFireRate(float refireTime)
    {
        canShoot = false;
        yield return new WaitForSeconds(refireTime);
        canShoot = true;
    }
}
