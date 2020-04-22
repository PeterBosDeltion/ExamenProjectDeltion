using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI GameOverStatusText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killsText;

    public void ActivateUI(bool victory)
    {
        gameObject.SetActive(true);

        SetTextValues(victory);
    }

    public void SetTextValues(bool victory)
    {
        if (victory)
            GameOverStatusText.text = "Victory!";
        else
            GameOverStatusText.text = "Mission Failed";
    }

    public void CloseGame()
    {
        GameManager.instance.CloseGame();
    }

    public void BackToMenu()
    {
        GameManager.instance.ToggleTimeScale();
        GameManager.instance.ChangeScene(0);
    }
}
