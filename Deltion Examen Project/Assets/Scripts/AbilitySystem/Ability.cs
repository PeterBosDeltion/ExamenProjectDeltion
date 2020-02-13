using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Player myPlayer;
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

    protected Coroutine afterDurCoroutine;

    private void Awake()
    {
        myPlayer = GetComponent<Player>();
    }
    public void UseAbility()
    {
        if (!onCooldown)
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

            StartCoroutine(Cooldown(cooldownTime));
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

    protected abstract IEnumerator AfterDuration();
}
