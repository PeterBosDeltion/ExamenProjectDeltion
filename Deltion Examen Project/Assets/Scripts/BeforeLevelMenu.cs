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
    private List<hGamepad> connectedPlayers = new List<hGamepad>();
    public void OpenMenu(int newSelectedLevel)
    {
        selectedLevel = newSelectedLevel;
        //GameObject cursor = Instantiate(multiplayerCursorPrefab, transform);
    }

    public void LaunchGame()
    {
        GameManager.instance.ChangeScene(selectedLevel);
    }

    private void Update()
    {
        //if (hinput.anyGamepad.start.justPressed)
        //    AddPlayer(GameManager.instance.amountOfPlayers);
        if (hinput.gamepad[0].start.justPressed && GameManager.instance.mouseKeyboardPlayer == 0)
            GameManager.instance.mouseKeyboardPlayer = 4;
        if (hinput.gamepad[1].start.justPressed)
            AddPlayer(1);
        if (hinput.gamepad[2].start.justPressed)
            AddPlayer(2);
        if (hinput.gamepad[3].start.justPressed)
            AddPlayer(3);
    }

    public void AddPlayer(int index)
    {
       

        //for (int i = 0; i < GameManager.instance.amountOfPlayers-1; i++)
        //{
        //    if (i == GameManager.instance.mouseKeyboardPlayer)
        //    {
        //        if(i < index)
        //        {
        //            index -= 1;
        //        }
        //    }
        //}

        //if (connectedPlayers.Contains(hinput.gamepad[index]))
        //    return;

        if (GameManager.instance.amountOfPlayers < 4)
        {
            GameManager.instance.amountOfPlayers++;
            playerLoadouts[index].SetActive(true);
            //if (!connectedPlayers.Contains(hinput.gamepad[index]))
            //    connectedPlayers.Add(hinput.gamepad[index]);
        }
    }
}
