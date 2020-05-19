using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFunctions : MonoBehaviour
{
    public PlayerController player;

    private bool wPressed;
    private bool aPressed;
    private bool sPressed;
    private bool dPressed;

    public bool allDirectionsPressed;
    public bool gunEmpty;
    public bool reloaded;
    public bool switched;
    public bool healed;

    public void AssignDelegates()
    {
        player = FindObjectOfType<PlayerController>();
        player.myInputManager.MovingEvent += DetectMovement;
        player.myInputManager.reloadEvent += Reload;
        player.myInputManager.LastWeaponEvent += Switch;
        player.myInputManager.scrollEvent += SwitchScroll;
        DisableAbilities();
        TutorialSystemManager.instance.steps[0].StartStep();

    }

    private void Update()
    {
        if (allDirectionsPressed && TutorialSystemManager.instance.currentStep == 0)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
        }
        if(!gunEmpty && TutorialSystemManager.instance.currentStep == 1)
        {
            if (player.currentWeapon.magazineAmmo <= 0)
            {
                gunEmpty = true;
                TutorialSystemManager.instance.CompleteCurrentStep();

            }
        }
        else if(TutorialSystemManager.instance.currentStep != 1)
        {
            if (player)
            {
                if (player.currentWeapon.canShoot)
                    player.currentWeapon.canShoot = false;
            }
        }
        if (reloaded && TutorialSystemManager.instance.currentStep == 2)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
        }
        if (switched && TutorialSystemManager.instance.currentStep == 3)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
        }
        if (!healed && TutorialSystemManager.instance.currentStep == 4)
        {
            if(player.GetComponent<Player>().GetHp() >= player.GetComponent<Player>().maxHp)
            {
                healed = true;
                TutorialSystemManager.instance.CompleteCurrentStep();

            }
        }
    }

    private void OnDestroy()
    {
        player.myInputManager.MovingEvent -= DetectMovement;
        player.myInputManager.reloadEvent -= Reload;
        player.myInputManager.LastWeaponEvent -= Switch;
        player.myInputManager.scrollEvent -= SwitchScroll;
    }

    private void SwitchScroll(float f)
    {
        Switch();
    }

    private void Switch()
    {
        if (!switched && TutorialSystemManager.instance.currentStep == 3)
            switched = true;

    }
    private void Reload()
    {
        Debug.Log("reached 1");
        if (!reloaded && TutorialSystemManager.instance.currentStep == 2)
        {
            Debug.Log("reached 2");
            reloaded = true;
        }

    }


    private void DetectMovement(float x, float z)
    {
        if (!allDirectionsPressed && TutorialSystemManager.instance.currentStep == 0)
        {
            if (z >= 1)
                wPressed = true;
            if (z <= -1)
                sPressed = true;

            if (x >= 1)
                dPressed = true;
            if (x <= -1)
                aPressed = true;

            if (wPressed && aPressed && sPressed && dPressed)
                allDirectionsPressed = true;
        }
        else
        {
            player.myInputManager.MovingEvent -= DetectMovement;
        }
    }

    public void DamagePlayer()
    {
        player.GetComponent<Player>().TakeDamage(500, null);
    }

    public void ResetCanShoot()
    {
        player.currentPrimary.canShoot = true;
        player.currentSecondary.canShoot = true;
    }

    public void DisableAbilities()
    {
        foreach (Ability a in player.abilities)
        {
            a.cantUse = true;
        }
    }

    public void EnableAbilities()
    {
        foreach (Ability a in player.abilities)
        {
            a.cantUse = false;
        }
    }
}
