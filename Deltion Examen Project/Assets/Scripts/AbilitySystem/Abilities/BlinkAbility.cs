using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAbility : Ability
{
    public float blinkDistance;
    public LayerMask floor;

    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3();

        if (x > 0)
        {
            dir.x = 0.5F;
        }
        else if(x < 0)
        {
            dir.x = -0.5F;
        }
        else
        {
            dir.x = 0;
        }

        if (z > 0)
        {
            dir.z = 0.5F;
        }
        else if (z < 0)
        {
            dir.z = -0.5F;
        }
        else
        {
            dir.z = 0;
        }

        Vector3 pos = myPlayer.transform.position + -dir * blinkDistance;
        Collider[] hitColliders = Physics.OverlapSphere(pos + new Vector3(0, .5F, 0), .25F);
        List<Collider> actualhitColliders = new List<Collider>();
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(pos + new Vector3(0, .5F, 0), Vector3.down, out hit, Mathf.Infinity))
        {
            if(hit.transform != null)
            {
                foreach (var item in hitColliders)
                {
                    if (!actualhitColliders.Contains(item))
                    {
                        if (item.transform.gameObject.layer != floor)
                        {
                            if (!item.isTrigger)
                            {
                                actualhitColliders.Add(item);
                                Debug.Log(item.transform.name);
                            }
                        }
                    }
                }
                if (actualhitColliders.Count <= 0)
                {
                    myPlayer.transform.position = pos;
                }
            }
        }
       



        active = true;
    }

    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        StartCooldown();
    }
}
