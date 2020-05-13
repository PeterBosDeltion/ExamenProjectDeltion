using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject loadoutMenu;
    public GameObject settingsMenu;
    public GameObject profileMenu;
    public GameObject beforeLevelMenu;

    public GameObject tutorialReminderWindow;
    public GameObject playButton;
    public GameObject firstLevelButton;

    private List<GameObject> menus = new List<GameObject>();
    public List<GameObject> mainButtons = new List<GameObject>();
    private GameObject currentOpenMenu;

    private void Start()
    {
        Initialize();
    }

    public void SetSelectedObject(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(selected);
    }

    private void Initialize()
    {
        if (mainMenu) //[PH] probably gonna check if scene is main menu instead
        {
            menus.Add(mainMenu);
        }
        if (levelMenu)
        {
            menus.Add(levelMenu);
        }
        if (loadoutMenu)
        {
            menus.Add(loadoutMenu);
        }
        if (settingsMenu)
        {
            menus.Add(settingsMenu);
        }
        if (profileMenu)
        {
            menus.Add(profileMenu);
        }
        if (beforeLevelMenu)
        {
            menus.Add(beforeLevelMenu);
        }

        //if not done tutorial
        if (tutorialReminderWindow.GetComponent<TutorialReminderWindow>().remindme)
        {
            tutorialReminderWindow.SetActive(true);
            SetSelectedObject(tutorialReminderWindow.GetComponent<TutorialReminderWindow>().yesButton);
        }
        else
        {
            SetSelectedObject(playButton);

        }
        tutorialReminderWindow.GetComponent<TutorialReminderWindow>().manager = this;



    }
    public void MenuButtonClicked(string button)
    {
        foreach (GameObject b in mainButtons)
        {
            b.SetActive(true);
        }
        switch (button)
        {
            case "Main":
                mainMenu.SetActive(true);
                currentOpenMenu = mainMenu;
                SetSelectedObject(playButton);
                break;
            case "Play":
                levelMenu.SetActive(true);
                currentOpenMenu = levelMenu;
                //SetSelectedObject(firstLevelButton);
                break;
            case "Loadout":
                loadoutMenu.SetActive(true);
                currentOpenMenu = loadoutMenu;
                break;
            case "Settings":
                settingsMenu.SetActive(true);
                currentOpenMenu = settingsMenu;
                break;
            case "Profile":
                profileMenu.SetActive(true);
                currentOpenMenu = profileMenu;
                break;
          
              
                break;
            default:
                Debug.LogError("Button input string not recognized");
                break;
        }

        CloseAllNotOpenMenus();
    }

    public void LevelButtonClicked(int level)
    {
        foreach (GameObject button in mainButtons)
        {
            button.SetActive(false);
        }
        beforeLevelMenu.SetActive(true);
        currentOpenMenu = beforeLevelMenu;

        BeforeLevelMenu lvlMenu = currentOpenMenu.GetComponent<BeforeLevelMenu>();
        lvlMenu.OpenMenu((int)level);
        SetSelectedObject(lvlMenu.startButton);

        CloseAllNotOpenMenus();
    }

    public void ReturnToMain()
    {
        MenuButtonClicked("Main");
    }

    private void CloseAllNotOpenMenus()
    {
        foreach (GameObject g in menus)
        {
            if(g != currentOpenMenu)
            {
                g.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        GameManager.instance.CloseGame();
    }
}
