using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalDrone : MonoBehaviour
{
    private float myHealpool;
    private Player myPlayer;
    private Vector3 targetPos;

    public float speed = 5;
    public float yOffset = 2.5F;
    public float zOffset = 1.5F;

    private LineRenderer lineRend;
    public float lineOffsetDrone;
    public Vector3 lineOffsetPlayer;
    public GameObject childDroneModel;

    public float healRate = 0.5F;

    private bool shouldFollow;
    private bool landed;

    public void Initialize(float healpool, Player player)
    {
        landed = false;
        myHealpool = healpool;
        myPlayer = player;

        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
        shouldFollow = true;

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
        else if(!shouldFollow && !landed)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                transform.position = Vector3.Lerp(transform.position, hit.point, speed * Time.deltaTime);
                if(Vector3.Distance(transform.position, hit.point) < .05F)
                {
                    landed = true;
                    GetComponent<Animator>().enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        if(myPlayer.hp < myPlayer.maxHp)
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

        if(myHealpool <= 0)
        {
            lineRend.enabled = false;
            shouldFollow = false;
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

        if(myHealpool > 0)
        {
            myHealpool -= healRate;
            myPlayer.hp += healRate;
            Debug.Log(myHealpool);
        }

    }
}
