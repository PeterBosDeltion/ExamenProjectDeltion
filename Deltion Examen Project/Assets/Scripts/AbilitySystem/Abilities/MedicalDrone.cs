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

    public Color fullColor;
    public Color emptyColor;

    public Image filledBatterImg;
    public Image outerBatterImg;
    private bool flashing;
    private float rate;

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

        filledBatterImg.color = fullColor;
        outerBatterImg.color = fullColor;

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
        if(myPlayer.GetHp() < myPlayer.maxHp) //&& myPlayer.GetHp() > 0
        {
            if (!reloading)
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

        if(reloading)
        {
            if (rate < myReloadSpeed)
            {
                rate += Time.deltaTime / myReloadSpeed;
                currentHealpool = Mathf.Lerp(0, myHealpool, rate);
            }
        }

        filledBatterImg.fillAmount = currentHealpool / myHealpool;

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
        currentHealpool = 0;
        rate = 0;
        filledBatterImg.color = emptyColor;
        outerBatterImg.color = emptyColor;
       
        yield return new WaitUntil(() => currentHealpool >= myHealpool);
        filledBatterImg.color = fullColor;
        outerBatterImg.color = fullColor;
        reloading = false;
    }

}