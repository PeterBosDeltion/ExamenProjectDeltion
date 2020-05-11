using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveBar : MonoBehaviour
{
    public GameObject objectiveIconsParent;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    private int currentProgress = 0;
    private int targetProgress;

    public GameObject objectiveIconPrefab;
    private List<GameObject> icons = new List<GameObject>();

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            pc.myInputManager.tabEvent += FadeTrue;
            pc.myInputManager.tabUpEvent += FadeFalse;
        }
    }

    private void OnDestroy()
    {
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            pc.myInputManager.tabEvent -= FadeTrue;
            pc.myInputManager.tabUpEvent -= FadeFalse;
        }
    }
    public void SetObjectiveDestroy(string description, int amountToDestroy)
    {
        icons.Clear();
        currentProgress = 0;
        targetProgress = amountToDestroy;
        descriptionText.text = description;
        progressText.text = currentProgress + "/" + targetProgress;

        for (int i = 0; i < amountToDestroy; i++)
        {
            GameObject icon = Instantiate(objectiveIconPrefab, objectiveIconsParent.transform);
            icons.Add(icon);
        }
    }

    public void AdvanceDestroyObjective()
    {
        currentProgress++;
        progressText.text = currentProgress + "/" + targetProgress;

        for (int i = 0; i < currentProgress; i++)
        {
            icons[i].GetComponent<ObjectiveIcon>().Complete();
        }
    }

    private void FadeTrue()
    {
        anim.SetBool("Fade", true);
    }

    private void FadeFalse()
    {
        anim.SetBool("Fade", false);
    }
}
