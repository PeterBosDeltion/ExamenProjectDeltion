using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialReminderWindow : MonoBehaviour
{
    public GameObject yesButton;
    [HideInInspector]
    public UIManager manager;

    public void Yes()
    {
        GameManager.instance.ChangeScene(1);
    }

    public void No()
    {
        gameObject.SetActive(false);
        manager.SetSelectedObject(manager.playButton);
    }

    public void NoDontRemind()
    {
        gameObject.SetActive(false);
        manager.SetSelectedObject(manager.playButton);
        PlayerProfile.instance.SetDoneTutorial(true);
    }
}
