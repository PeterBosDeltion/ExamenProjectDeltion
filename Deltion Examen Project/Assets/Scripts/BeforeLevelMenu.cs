using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeLevelMenu : MonoBehaviour
{
    private int selectedLevel;
    public GameObject startButton;

    public void OpenMenu(int newSelectedLevel)
    {
        selectedLevel = newSelectedLevel;
    }

    public void LaunchGame()
    {
        GameManager.instance.ChangeScene(selectedLevel);
    }
}
