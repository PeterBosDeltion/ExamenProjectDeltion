using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private PauseMenu pauseMenu;
    void Start()
    {
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
        pauseMenu.Initialize();
    }
}
