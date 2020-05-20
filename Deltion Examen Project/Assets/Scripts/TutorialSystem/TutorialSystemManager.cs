using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TutorialSystemManager : MonoBehaviour
{
    public static TutorialSystemManager instance;
    public int currentStep = -1;
    public float betweenStepsTime = 1;
    public List<TutorialStep> steps = new List<TutorialStep>();
    [HideInInspector]
    public bool waiting;
    public TutorialFunctions functions;

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
        functions = GetComponent<TutorialFunctions>();
    }
    public void NextStep()
    {
        if(currentStep < steps.Count)
            currentStep++;

    }

    public void CompleteCurrentStep()
    {
        steps[currentStep].Complete();
        NextStep();

        if (!waiting)
            StartCoroutine(WaitBeforeNextStep());
    }

    private IEnumerator WaitBeforeNextStep()
    {
        waiting = true;
        yield return new WaitForSeconds(betweenStepsTime);
        if(currentStep < steps.Count)
        {
            steps[currentStep].StartStep();
            functions.doneStepToggle.isOn = false;
        }
        waiting = false;
    }

}
[System.Serializable]
public class TutorialStep
{
    public bool completed = false;
    public List<GameObject> activatedObjects = new List<GameObject>();

    public string stepTextDescription;
    public string stepDoneDescription;

    public TextMeshProUGUI textObject;

    public List<UnityEvent> CalledUponCompletion = new List<UnityEvent>();

    public void StartStep()
    {
        textObject.text = stepTextDescription;
        foreach (GameObject toActivate in activatedObjects)
        {
            toActivate.SetActive(true);
        }
    }
    public void Complete()
    {
        textObject.text = stepDoneDescription;
        foreach (GameObject toActivate in activatedObjects)
        {
            toActivate.SetActive(false);
        }

        foreach (UnityEvent e in CalledUponCompletion)
        {
            e.Invoke();
        }
        completed = true;
    }


}
