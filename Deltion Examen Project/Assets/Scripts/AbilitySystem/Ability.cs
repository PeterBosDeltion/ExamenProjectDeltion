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
            switch (myDeployType)
            {
                case DeployType.Instant:
                    AbilityMechanic();
                    break;
                case DeployType.Deployed:
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        Vector3 mPos = hit.point;
                        if (Vector3.Distance(transform.position, mPos) <= maxDeployableDistance)
                        {
                            AbilityMechanic();
                        }
                    }
                    break;
                case DeployType.LaserTarget:
                    break;
            }

            afterDurCoroutine = StartCoroutine(AfterDuration());
        }
       
    }

    protected abstract void AbilityMechanic();

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
