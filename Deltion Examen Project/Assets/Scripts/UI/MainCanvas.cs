using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private PauseMenu pauseMenu;
    [HideInInspector]
    public GameOverScreen gameOverScreen;
    void Start()
    {
        gameOverScreen = GetComponentInChildren<GameOverScreen>(true);
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
        pauseMenu.Initialize();
    }
}