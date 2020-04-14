using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject settingsScreen;

    public void Initialize()
    {
        InputManager.escapeEvent += OpenClose;
    }

    private void OnDestroy()
    {
        InputManager.escapeEvent -= OpenClose;
    }
    private void OpenClose()
    {
        
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameObject.SetActive(!gameObject.activeSelf);
        float newTS = (gameObject.activeSelf) ? 0 : 1;
        Time.timeScale = newTS;
    }

    public void Close()
    {
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenScreen(string screen)
    {
        switch (screen)
        {
            case "Main":
                settingsScreen.SetActive(false);
                mainScreen.SetActive(true);
                break;
            case "Settings":
                settingsScreen.SetActive(true);
                mainScreen.SetActive(false);
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.ChangeScene(0);
    }
}
