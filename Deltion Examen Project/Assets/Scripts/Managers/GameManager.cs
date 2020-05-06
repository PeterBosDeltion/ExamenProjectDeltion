﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void CursorChange();
    public static CursorChange cursorEvent;

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    public enum CursorState
    {
        Cursor,
        Crosshair,
        Empty
    }

    public GameState curentState;
    public CursorState curentCursorState;

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

    public void SetCursorState(CursorState state)
    {
        curentCursorState = state;
        cursorEvent.Invoke();
    }

    public void SetGameState(GameState state)
    {
        curentState = state;
        if(state == GameState.GameOver || state == GameState.Paused)
            SetCursorState(CursorState.Cursor);
        else
            SetCursorState(CursorState.Crosshair);
    }

    public void ChangeScene(int index)
    {
        cursorEvent = null;

        if (index != 0)
            curentCursorState = CursorState.Crosshair;
        else
            curentCursorState = CursorState.Cursor;

        SceneManager.LoadScene(index);
    }

    public void CheckGameOver()
    {
        if(amountOfPlayers > 1)
        {
            if(playerOne.GetIfDeath() && playerTwo.GetIfDeath() && playerThree.GetIfDeath() && playerFour.GetIfDeath())
            {
                GameOver(false);
            }
        }
        else
        {
            GameOver(false);
        }
    }

    public void GameOver(bool victory)
    {
        SetGameState(GameState.GameOver);
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