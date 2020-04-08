using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public delegate void TutorialStepOne();
    public static TutorialStepOne stepOneDelegate;
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
        AddEmptyDelegates();
        stepOneDelegate += StepOne;

        stepOneDelegate.Invoke();
    }

    private void AddEmptyDelegates()
    {
        stepOneDelegate += Empty;
    }

    private void Empty()
    {

    }

    private void StepOne() //WASD movement
    {
        tutorialText.text = tutorialTexts[0];
    }
}
