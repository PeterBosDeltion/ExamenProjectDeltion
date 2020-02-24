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

    private List<GameObject> menus = new List<GameObject>();
    private GameObject currentOpenMenu;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        menus.Add(mainMenu);
        menus.Add(levelMenu);
        //menus.Add(loadoutMenu);
        //menus.Add(settingsMenu);
        //menus.Add(profileMenu);
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
            case "Loudout":
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
            default:
                Debug.LogError("Button input string not recognized");
                break;
        }

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
