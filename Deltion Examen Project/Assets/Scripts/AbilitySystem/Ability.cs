using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    private int requiredLevel;
    public bool ultimate;
    protected bool onCooldown;
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

    public void UseAbility()
    {
        switch (myDeployType)
        {
            case DeployType.Instant:
                AbilityMechanic();
                break;
            case DeployType.Deployed:
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
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
    }

    protected abstract void AbilityMechanic();
}
