using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryHealingStation : Interactable
{
    public float totalAvailableHealth = 500;
    public float healRate;
    private float currentAvailableHealth;

    private bool canHeal = false;
    public GameObject aoeImg;
    public Animator aoeAnim;
    private bool waiting;
    private Coroutine waitingForAnim;

    public GameObject healthCylinder;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentAvailableHealth = totalAvailableHealth;
    }
    protected override void Interact()
    {
        if (waitingForAnim != null)
        {
            StopCoroutine(waitingForAnim);
            waiting = false;
        }
        interacted = true;
        canHeal = true;

        aoeImg.SetActive(true);
        aoeAnim.Play("Open", 0);
    }

    private void Heal()
    {
        if (currentAvailableHealth > 0)
        {
            currentAvailableHealth -= healRate;
            foreach (Player player in nearbyPlayers)
            {
                if(player.GetHp() < player.maxHp && player.GetHp() > 0)
                    player.Heal(healRate, 0);
            }
        }
    }

    private void Update()
    {
        if (currentAvailableHealth > 0 && canHeal)
        {
            if (nearbyPlayers.Count > 0)
            {
                Heal();
                healthCylinder.transform.localScale = new Vector3(healthCylinder.transform.localScale.x, healthCylinder.transform.localScale.y, currentAvailableHealth / totalAvailableHealth);
            }
            else
            {
                canHeal = false;
                aoeAnim.Play("Close", 0);
                if (!waiting)
                {
                    waitingForAnim = StartCoroutine(WaitForAnimClose());
                }

            }
        }
        else if(currentAvailableHealth <= 0)
        {
            canHeal = false;
            aoeAnim.Play("Close", 0);
            if (!waiting)
            {
                waitingForAnim = StartCoroutine(WaitForAnimClose());
            }

            interactText.gameObject.SetActive(false);

            canInteract = false;
        }

    }

    private IEnumerator WaitForAnimClose()
    {
        waiting = true;
        yield return new WaitForSeconds(aoeAnim.GetCurrentAnimatorStateInfo(0).length);
        aoeImg.SetActive(false);
        waiting = false;
    }


}
