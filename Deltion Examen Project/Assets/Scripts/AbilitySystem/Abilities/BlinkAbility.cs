using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAbility : Ability
{
    public float blinkDistance;
    public LayerMask floor;
    private bool blinked;
    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        blinked = false;
        Vector3 pos = myPlayer.transform.position + myPlayer.transform.forward * blinkDistance;
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
                    blinked = true;
                    active = true;
                }
                else
                {
                    blinked = false;
                    returned = true;
                }

            }
            else if(hit.transform == null)
            {
                blinked = false;
                returned = true;
            }
        }
    }

    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        if(blinked)
            StartCooldown();
    }
}
