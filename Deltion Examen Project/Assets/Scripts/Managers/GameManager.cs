using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int difficulty = 2;
    public int amountOfPlayers = 1;

    public PlayerController[] GetPlayers()
    {
        return FindObjectsOfType<PlayerController>();
    }

    public PlayerController playerOne;
    public PlayerController playerTwo;
    public PlayerController playerThree;
    public PlayerController playerFour;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(transform.root.gameObject);
        }

        DontDestroyOnLoad(transform.root.gameObject);
    }
    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void GameOver(bool victory)
    {
        GameOverScreen gameOverUI = FindObjectOfType<MainCanvas>().gameOverScreen;
        if(victory)
        {
            gameOverUI.ActivateUI(true);
            Debug.Log("Victory");
        }
        else
        {
            gameOverUI.ActivateUI(false);
            Debug.Log("Loss");
        }
        ToggleTimeScale();
    }

    public void ToggleTimeScale()
    {
        if(Time.timeScale == 1)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}