using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject loadoutMenu;
    public GameObject settingsMenu;
    public GameObject profileMenu;
    public GameObject beforeLevelMenu;

    private List<GameObject> menus = new List<GameObject>();
    private GameObject currentOpenMenu;

    private void Start()
    {
        Initialize();
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

    }
    public void MenuButtonClicked(string button)
    {
        switch (button)
        {
            case "Main":
                mainMenu.SetActive(true);
                currentOpenMenu = mainMenu;
                break;
            case "Play":
                levelMenu.SetActive(true);
                currentOpenMenu = levelMenu;
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
        beforeLevelMenu.SetActive(true);
        currentOpenMenu = beforeLevelMenu;

        BeforeLevelMenu lvlMenu = currentOpenMenu.GetComponent<BeforeLevelMenu>();
        lvlMenu.OpenMenu((int)level);

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
        Application.Quit();
    }
}
