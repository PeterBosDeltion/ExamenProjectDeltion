using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public float timeBetweenSteps = 4;
    public Image checkmark;
    private bool timing;
    private bool waiting;
    public int currentStep;
    public delegate void TutorialStep();
    public static TutorialStep stepOneDelegate;
    public static TutorialStep playerMovedDelegate;
    public List<string> tutorialTexts = new List<string>();
    public TextMeshProUGUI tutorialText;

    private GameObject currentIndicator;
    public GameObject mainUIIndicator;
    public GameObject weaponIndicator;
    public GameObject healthIndicator;
    public GameObject abilitiesIndicator;
    public GameObject ultimateIndicator;

    public GameObject tutorialEnemy;
    public GameObject tutorialInteractable;
    public GameObject damageMatrix;
    public GameObject healingMatrix;

    public PlayerController player;
    private bool playerMoved = false;
    private bool playerRotated = false;
    private bool playerFired = false;
    private bool playerReloaded = false;
    private bool playerSwitched = false;
    private bool playerDamaged = false;
    private bool playerHealed = false;
    private bool playerUsedFirstAbility = false;
    private bool playerSeenCooldown = false;
    private bool playerUsedUlt = false;
    private bool playerHasInteracted = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentStep = 1;
        player.inTutorial = true;
       

        AddEmptyDelegates();
        stepOneDelegate += StepOne;
        playerMovedDelegate += PlayerMoved;
        InputManager.RotatingEvent += PlayerRotated;
        InputManager.leftMouseButtonHoldEvent += PlayerFired;
        InputManager.reloadEvent += PlayerReloaded;
        InputManager.scrollEvent += PlayerSwitched;
        InputManager.abilityEvent += AbilityOneUsed;
        InputManager.abilityEvent += UltUsed;

        stepOneDelegate.Invoke();
    }

    private void AddEmptyDelegates()
    {
        stepOneDelegate += Empty;
        playerMovedDelegate += Empty;
    }

    private void Empty()
    {

    }
    private void NextStep()
    {
        currentStep++;
        switch (currentStep)
        {
            case 1: //Ask for movement
                StepOne();
                break;
            case 2:
                if (playerMoved && !timing) //Player has moved, ask for rotation
                    tutorialText.text = tutorialTexts[1];
                break;
            case 3:
                if (playerRotated && !timing)//Player has rotated, ask for firing weapon
                {
                    tutorialText.text = tutorialTexts[2];
                    player.loadout.primary.tutorialInit = true;
                    player.loadout.secondary.tutorialInit = true;
                    player.loadout.primary.Initialize();
                    player.loadout.secondary.Initialize();
                }
                break;
            case 4: //Player has fired, explain GUI and ask for reload
                if (playerFired && !timing)
                {
                    tutorialText.text = tutorialTexts[3];
                    currentIndicator = mainUIIndicator;
                    currentIndicator.SetActive(true);

                }
                break;
            case 5: //Player has reloaded, explain ammo & weapon GUI and ask for switch
                if (playerReloaded && !timing)
                {
                    tutorialText.text = tutorialTexts[4];
                    currentIndicator.SetActive(false);
                    currentIndicator = weaponIndicator;
                    currentIndicator.SetActive(true);

                }
                break;
            case 6: //Player has switched weapon, explain health GUI
                if (playerSwitched && !timing)
                {
                    tutorialText.text = tutorialTexts[5];
                    currentIndicator.SetActive(false);
                    currentIndicator = healthIndicator;
                    currentIndicator.SetActive(true);
                    damageMatrix.SetActive(true);
                }
                break;
            case 7: //Player has been damaged, explain healing
                if (playerDamaged && !timing)
                {
                    damageMatrix.SetActive(false);
                    tutorialText.text = tutorialTexts[6];
                    currentIndicator.SetActive(false);
                    currentIndicator = healthIndicator;
                    currentIndicator.SetActive(true);
                    healingMatrix.SetActive(true);
                }
                break;
            case 8: //Player has been healed, explain abilities
                if (playerHealed && !timing)
                {
                    player.tutorialAbilityInit = true;
                    healingMatrix.SetActive(false);
                    tutorialText.text = tutorialTexts[7];
                    currentIndicator.SetActive(false);
                    currentIndicator = abilitiesIndicator;
                    currentIndicator.SetActive(true);
                }
                break;
            case 9: //Player has used ability, explain cooldown
                if (playerUsedFirstAbility && !timing)
                {
                    tutorialText.text = tutorialTexts[8];
                    currentIndicator.SetActive(false);
                    currentIndicator = abilitiesIndicator;
                    currentIndicator.SetActive(true);
                    if (!waiting)
                        StartCoroutine(WaitAfterAbility());
                }
                break;
            case 10: //Player has seen cooldown, explain ultimate
                if (playerSeenCooldown && !timing)
                {
                    tutorialText.text = tutorialTexts[9];
                    currentIndicator.SetActive(false);
                    currentIndicator = ultimateIndicator;
                    currentIndicator.SetActive(true);
                    tutorialEnemy.SetActive(true);
                   
                }
                break;
            case 11: //Player has used ult, explain interactables
                if (playerUsedUlt && !timing)
                {
                    tutorialText.text = tutorialTexts[10];
                    currentIndicator.SetActive(false);
                    currentIndicator = null;
                    tutorialEnemy.SetActive(false);
                    tutorialInteractable.SetActive(true);

                }
                break;
            case 12: //Player has used interacted, tutorial completed.
                if (playerHasInteracted && !timing)
                {
                    tutorialText.text = tutorialTexts[11];
                    tutorialInteractable.SetActive(false);

                }
                break;
        }
    }
    private void StepOne() //WASD movement
    {
        tutorialText.text = tutorialTexts[0];
    }

    private void PlayerMoved()
    {
        playerMoved = true;
        if (!timing && currentStep == 1)
        {
            StartCoroutine(BetweenStepTimer());
        }
    }

    private void PlayerRotated(float x, float y)
    {
        playerRotated = true;
        if(!timing && currentStep == 2)
        {
            InputManager.RotatingEvent -= PlayerRotated;
            StartCoroutine(BetweenStepTimer());
        }
    }

    private void PlayerFired()
    {
        playerFired = true;
        if (!timing && currentStep == 3)
        {
            InputManager.leftMouseButtonHoldEvent -= PlayerFired;
            StartCoroutine(BetweenStepTimer());
        }
    }
    private void PlayerReloaded()
    {
        playerReloaded = true;
        if (!timing && currentStep == 4)
        {
            InputManager.reloadEvent -= PlayerReloaded;
            StartCoroutine(BetweenStepTimer());
        }
    }

    private void PlayerSwitched(float f)
    {
        playerSwitched = true;
        if (!timing && currentStep == 5)
        {
            InputManager.scrollEvent -= PlayerSwitched;
            StartCoroutine(BetweenStepTimer());
        }
    }

    public void PlayerDamaged()
    {
        playerDamaged = true;
        if (!timing && currentStep == 6)
        {
            StartCoroutine(BetweenStepTimer());
        }
    }

    public void PlayerHealed()
    {
        playerHealed = true;
        if (!timing && currentStep == 7)
        {
            StartCoroutine(BetweenStepTimer());
        }

    }

    private void AbilityOneUsed(int i)
    {
        if(i == 0) //First ability used
        {
            playerUsedFirstAbility = true;
            if (!timing && currentStep == 8)
            {
                InputManager.abilityEvent -= AbilityOneUsed;
                StartCoroutine(BetweenStepTimer());
            }
        }
    }

    private void UltUsed(int i)
    {
        if(i == 4) //Ultimate 
        {
            if(player.ultimateAbility.active)
            {
                playerUsedUlt = true;
                if (!timing && currentStep == 10)
                {
                    InputManager.abilityEvent -= UltUsed;
                    StartCoroutine(BetweenStepTimer());
                }
            }
        }
    }

    public void Interacted()
    {
        playerHasInteracted = true;
        if (!timing && currentStep == 11)
        {
            StartCoroutine(BetweenStepTimer());
        }
    }

    private IEnumerator WaitAfterAbility()
    {
        waiting = true;
        yield return new WaitForSeconds(timeBetweenSteps * 2);
        playerSeenCooldown = true;
        StartCoroutine(BetweenStepTimer());
        waiting = false;
    }

    private IEnumerator BetweenStepTimer()
    {
        timing = true;
        checkmark.enabled = true;
        yield return new WaitForSeconds(timeBetweenSteps);
        checkmark.enabled = false;
        timing = false;
        NextStep();
    }
}
