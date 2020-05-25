using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class BeforeLevelMenu : MonoBehaviour
{
    private int selectedLevel;
    public GameObject startButton;
    public GameObject multiplayerCursorPrefab;
    public List<GameObject> playerLoadouts = new List<GameObject>();
    private List<string> connectedPlayers = new List<string>();

    private int currentIndex = -1;
    public void OpenMenu(int newSelectedLevel)
    {
        currentIndex = -1;
        selectedLevel = newSelectedLevel;
        //GameObject cursor = Instantiate(multiplayerCursorPrefab, transform);

        foreach (GameObject g in playerLoadouts)
        {
            g.SetActive(false);
        }
        if(GameManager.instance.mouseKeyboardPlayer < 4)
        {
            playerLoadouts[GameManager.instance.mouseKeyboardPlayer].SetActive(true);
        }
        else
        {
            playerLoadouts[0].SetActive(true);
            connectedPlayers.Add(hinput.gamepad[0].name);
            currentIndex = 0;
        }

        if (GameManager.instance.mouseKeyboardPlayer != 0 && GameManager.instance.mouseKeyboardPlayer < 4)
        {
            AddPlayer();
            connectedPlayers.Add(hinput.gamepad[0].name);

        }

    }

    public void LaunchGame()
    {
        GameManager.instance.ChangeScene(selectedLevel);
    }

    private void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (hinput.gamepad[0].start.justPressed)
        {
            string gName = hinput.gamepad[0].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
        if (hinput.gamepad[1].start.justPressed)
        {
            string gName = hinput.gamepad[1].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
        if (hinput.gamepad[2].start.justPressed)
        {
            string gName = hinput.gamepad[2].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
        if (hinput.gamepad[3].start.justPressed)
        {
            string gName = hinput.gamepad[3].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
        if (hinput.gamepad[4].start.justPressed)
        {
            string gName = hinput.gamepad[4].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
        if (hinput.gamepad[5].start.justPressed)
        {
            string gName = hinput.gamepad[5].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
        if (hinput.gamepad[6].start.justPressed)
        {
            string gName = hinput.gamepad[6].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
        if (hinput.gamepad[7].start.justPressed)
        {
            string gName = hinput.gamepad[7].name;
            if (!connectedPlayers.Contains(gName))
            {
                connectedPlayers.Add(gName);
                AddPlayer();
            }
        }
    }

    public void AddPlayer()
    {
        if (GameManager.instance.amountOfPlayers < 4)
        {
            currentIndex++;
            if(currentIndex == GameManager.instance.mouseKeyboardPlayer)
            {
                if(currentIndex < 3)
                    currentIndex++;
            }
            GameManager.instance.amountOfPlayers++;
            playerLoadouts[currentIndex].SetActive(true);
        }
    }

    public void LoadoutBackButton()
    {
        FindObjectOfType<UIManager>().ReturnToMain();
        GameManager.instance.amountOfPlayers = 1;
        currentIndex = -1;
        connectedPlayers.Clear();
    }
}
