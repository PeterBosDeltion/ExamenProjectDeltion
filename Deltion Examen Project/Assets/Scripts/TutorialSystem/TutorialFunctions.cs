using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFunctions : MonoBehaviour
{
    public PlayerController player;

    private bool wPressed;
    private bool aPressed;
    private bool sPressed;
    private bool dPressed;
    private GameObject objective;

    public bool allDirectionsPressed;
    public bool gunEmpty;
    public bool reloaded;
    public bool switched;
    public bool healed;
    public bool abilityUsed;
    public bool abilityUsedAgain;
    public bool ultUsed;

    public bool nearObjective;
    public Toggle doneStepToggle;
    private TriggerHurt hurt;
    public void AssignDelegates()
    {
        player = FindObjectOfType<PlayerController>();
        doneStepToggle = FindObjectOfType<Toggle>();
        hurt = FindObjectOfType<TriggerHurt>();
        player.myInputManager.MovingEvent += DetectMovement;
        player.myInputManager.reloadEvent += Reload;
        player.myInputManager.LastWeaponEvent += Switch;
        player.myInputManager.scrollEvent += SwitchScroll;
        player.myInputManager.abilityEvent += AbilityUsed;
        player.canSwitch = false;
        objective = FindObjectOfType<DestroyObjective>().gameObject;
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
                doneStepToggle.isOn = true;

                TutorialSystemManager.instance.CompleteCurrentStep();

            }
        }
        else if(TutorialSystemManager.instance.currentStep != 1 && !gunEmpty)
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
            player.canSwitch = true;

        }
        if (switched && TutorialSystemManager.instance.currentStep == 3)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
        }
        if (!healed && TutorialSystemManager.instance.currentStep == 4)
        {
            if(player.GetComponent<Player>().GetHp() >= player.GetComponent<Player>().maxHp && hurt.damagedOnce)
            {
                healed = true;
                doneStepToggle.isOn = true;

                TutorialSystemManager.instance.CompleteCurrentStep();

            }
        }
        if (abilityUsed && TutorialSystemManager.instance.currentStep == 5)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
        }
        if (abilityUsedAgain && TutorialSystemManager.instance.currentStep == 6)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
            for (int i = 0; i < 100; i++)
            {
                player.ultimateAbility.IncrementUltCharge();
            }
        }
        if (ultUsed && TutorialSystemManager.instance.currentStep == 7)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
        }
        else if(!ultUsed && TutorialSystemManager.instance.currentStep == 7)
        {
            if (player.ultimateAbility.active)
            {
                doneStepToggle.isOn = true;
                ultUsed = true;

            }
        }
        if (!nearObjective && TutorialSystemManager.instance.currentStep == 8)
        {
            if (Vector3.Distance(player.transform.position, objective.transform.position) <= 15)
            {
                doneStepToggle.isOn = true;
                nearObjective = true;

            }
        }
        else if (nearObjective && TutorialSystemManager.instance.currentStep == 8)
        {
            TutorialSystemManager.instance.CompleteCurrentStep();
        }
    }

    private void OnDestroy()
    {
        player.myInputManager.MovingEvent -= DetectMovement;
        player.myInputManager.reloadEvent -= Reload;
        player.myInputManager.LastWeaponEvent -= Switch;
        player.myInputManager.scrollEvent -= SwitchScroll;
        player.myInputManager.abilityEvent -= AbilityUsed;
    }

    private void SwitchScroll(float f)
    {
        Switch();
    }

    private void Switch()
    {
        if (!switched && TutorialSystemManager.instance.currentStep == 3)
        {
            doneStepToggle.isOn = true;
            switched = true;
        }

    }
    private void Reload()
    {
        if (!reloaded && TutorialSystemManager.instance.currentStep == 2)
        {
            doneStepToggle.isOn = true;
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
            {
                doneStepToggle.isOn = true;
                allDirectionsPressed = true;

            }
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
        player.currentWeapon.canShoot = true;
    }

    public void DisableAbilities()
    {
        foreach (Ability a in player.abilities)
        {
            a.cantUse = true;
        }

        player.ultimateAbility.cantUse = true;
    }

    public void EnableAbilities()
    {
        if(TutorialSystemManager.instance.currentStep == 4)
        {
            player.abilities[0].cantUse = false;
        }
        else if(TutorialSystemManager.instance.currentStep == 5)
        {
            player.abilities[3].cantUse = false;
        }
        else
        {
            foreach (Ability a in player.abilities)
            {
                a.cantUse = false;
            }
        }
        
    }

    public void EnableUlt()
    {
        player.ultimateAbility.cantUse = false;
    }

    public void AbilityUsed(int ability)
    {
        if(ability == 0 && TutorialSystemManager.instance.currentStep == 5)
        {
            abilityUsed = true;
            doneStepToggle.isOn = true;

        }
        if (ability == 3 && TutorialSystemManager.instance.currentStep == 6)
        {
            abilityUsedAgain = true;
            doneStepToggle.isOn = true;

        }
    }

    public void SaveDoneTutorialTrue()
    {
        PlayerProfile.instance.SetDoneTutorial(true);
    }
}
