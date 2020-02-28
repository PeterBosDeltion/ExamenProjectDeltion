using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Player myPlayer;
    private int requiredLevel;
    public bool ultimate;
    public bool onCooldown;
    public bool active = false;
    public float cooldownTime;
    public float duration;
    protected float currentUltCharge;
    public float maxDeployableDistance = 15;

    [HideInInspector]
    public bool returned;
    public enum DeployType
    {
        Instant,
        Deployed,
        LaserTarget
    }
    public DeployType myDeployType;

    protected Coroutine afterDurCoroutine;

    public void UseAbility()
    {
        if (!onCooldown && !active)
        {
            returned = false;
            switch (myDeployType)
            {
                case DeployType.Instant:
                    AbilityMechanic();
                    break;
                case DeployType.Deployed:
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                    {
                        Debug.Log(hit.transform.gameObject.layer);
                        if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
                        {
                            Vector3 mPos = hit.point;
                            if (Vector3.Distance(transform.position, mPos) <= maxDeployableDistance)
                            {
                                AbilityMechanic(mPos);
                            }
                        }
                        else
                        {
                            onCooldown = false;
                            returned = true;
                            return;
                        }
                       
                    }
                    break;
                case DeployType.LaserTarget:
                    break;
            }

            afterDurCoroutine = StartCoroutine(AfterDuration());
        }
       
    }

    protected abstract void AbilityMechanic(Vector3? mPos = null);

    private IEnumerator Cooldown(float seconds)
    {
        onCooldown = true;
        yield return new WaitForSeconds(seconds);
        onCooldown = false;
    }

    protected void StartCooldown()
    {
        if (!this.onCooldown)
        {
            active = false;
            StartCoroutine(Cooldown(cooldownTime));
        }
    }

    protected abstract IEnumerator AfterDuration();
}
