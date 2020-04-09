using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public float timeBetweenSteps = 4;
    public Image uiTimer;
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
            case 1:
                StepOne();
                break;
            case 2:
                if (playerMoved && !timing)
                    tutorialText.text = tutorialTexts[1];
                break;
            case 3:
                if (playerRotated && !timing)
                    tutorialText.text = tutorialTexts[2];
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

    private IEnumerator BetweenStepTimer()
    {
        timing = true;
        yield return new WaitForSeconds(timeBetweenSteps);
        uiTimer.fillAmount = 1;
        timing = false;
        NextStep();
    }

    private void Update()
    {
        if (timing)
        {
            uiTimer.fillAmount -= Time.deltaTime / timeBetweenSteps;
        }
    }
}
