using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject settingsScreen;

    public void Initialize()
    {
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            pc.myInputManager.escapeEvent += OpenClose;
        }
    }

    private void OnDestroy()
    {
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            pc.myInputManager.escapeEvent -= OpenClose;
        }
    }
    private void OpenClose()
    {
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameObject.SetActive(!gameObject.activeSelf);
        GameManager.instance.ToggleTimeScale();

        if(gameObject.activeSelf)
            GameManager.instance.SetGameState(GameManager.GameState.Paused);
        else
            GameManager.instance.SetGameState(GameManager.GameState.Playing);
    }

    public void Close()
    {
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
        gameObject.SetActive(false);
        GameManager.instance.ToggleTimeScale();
        GameManager.instance.SetGameState(GameManager.GameState.Playing);
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
        GameManager.instance.CloseGame();
    }

    public void MainMenu()
    {
        GameManager.instance.ToggleTimeScale();
        GameManager.instance.ChangeScene(0);
    }
}
