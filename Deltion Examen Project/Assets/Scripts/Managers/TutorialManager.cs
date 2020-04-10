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

    public PlayerController player;
    private bool playerMoved = false;
    private bool playerRotated = false;
    private bool playerFired = false;
    private bool playerReloaded = false;
    private bool playerSwitched = false;
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
        player.loadout.primary.myPlayer = player.GetComponent<Player>();
        player.loadout.secondary.myPlayer = player.GetComponent<Player>();

        AddEmptyDelegates();
        stepOneDelegate += StepOne;
        playerMovedDelegate += PlayerMoved;
        InputManager.RotatingEvent += PlayerRotated;
        InputManager.leftMouseButtonHoldEvent += PlayerFired;
        InputManager.reloadEvent += PlayerReloaded;
        InputManager.scrollEvent += PlayerSwitched;


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
