using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private PauseMenu pauseMenu;
    public List<GameObject> playerUis = new List<GameObject>();
    [HideInInspector]
    public GameOverScreen gameOverScreen;
    void Start()
    {
        gameOverScreen = GetComponentInChildren<GameOverScreen>(true);
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
        pauseMenu.Initialize();
        PlayerUI[] pUIs = GetComponentsInChildren<PlayerUI>(true);

        foreach (var pui in pUIs)
        {
            playerUis.Add(pui.gameObject);
        }
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < GameManager.instance.amountOfPlayers; i++)
        {
            playerUis[i].SetActive(true);
        }
    }

    public void SetUIPlayers()
    {
        foreach (var ui in playerUis)
        {
            ui.GetComponent<PlayerUI>().GetMyPlayer();
        }
    }
}