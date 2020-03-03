using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicalDrone : MonoBehaviour
{
    private float myHealpool;
    private float currentHealpool;
    private Player myPlayer;
    private Vector3 targetPos;
    private float myReloadSpeed;
    private bool reloading;
    private float myHealrate;

    public float speed = 5;
    public float yOffset = 2.5F;
    public float zOffset = 1.5F;

    private LineRenderer lineRend;
    public float lineOffsetDrone;
    public Vector3 lineOffsetPlayer;
    public GameObject childDroneModel;
    private bool shouldFollow;

    public Color flashFrom;
    public Color flashTo;

    public Image batterImg;
    private bool flashing;

    public void Initialize(float healpool, Player player, float reloadSpeed, float healRate)
    {
        myHealpool = healpool;
        currentHealpool = myHealpool;
        myPlayer = player;
        myReloadSpeed = reloadSpeed;
        myHealrate = healRate;

        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
        shouldFollow = true;
        batterImg.gameObject.SetActive(false);

        if (!childDroneModel)
        {
            Debug.Log("Please assign the child model of the medical drone");
        }
    }

    private void FixedUpdate()
    {
        if (shouldFollow)
        {
            FollowPlayer();
        }
    }

    private void Update()
    {
        if(myPlayer.GetHp() < myPlayer.maxHp)
        {
            Heal();
        }
        else
        {
            if (lineRend.enabled)
            {
                lineRend.enabled = false;
            }
        }

        if(currentHealpool <= 0)
        {
            lineRend.enabled = false;
            if (!reloading)
            {
                StartCoroutine(ReloadPool());
            }
        }
    }
    private void FollowPlayer()
    {
        targetPos = myPlayer.transform.position + -myPlayer.transform.forward * zOffset + new Vector3(0, yOffset, 0);
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        Vector3 lookPos = myPlayer.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
    }

    private void Heal()
    {
        lineRend.SetPosition(0, childDroneModel.transform.position + transform.forward * lineOffsetDrone);
        lineRend.SetPosition(1, myPlayer.transform.position + lineOffsetPlayer);
        lineRend.enabled = true;

        if(currentHealpool > 0)
        {
            currentHealpool -= myHealrate;
            myPlayer.Heal(myHealrate, 0);
        }
    }

    private IEnumerator ReloadPool()
    {
        reloading = true;
        if (!flashing)
        {
            batterImg.gameObject.SetActive(true);
            batterImg.color = flashFrom;
            InvokeRepeating("FlashBattery", 0, 1);
        }
        yield return new WaitForSeconds(myReloadSpeed);
        flashing = false;
        CancelInvoke("FlashBattery");
        batterImg.gameObject.SetActive(false);
        currentHealpool = myHealpool;
        reloading = false;
    }

    private void FlashBattery()
    {
        flashing = true;
        batterImg.color = (batterImg.color == flashFrom) ? batterImg.color = flashTo : batterImg.color = flashFrom;
    }
}