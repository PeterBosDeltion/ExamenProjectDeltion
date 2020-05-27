using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI GameOverStatusText;
    public TextMeshProUGUI experienceText;
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

        experienceText.text = ExperienceManager.instance.xpGained.ToString();
        killsText.text = EntityManager.instance.killedEnemies.ToString();
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

    public void RestartLevel()
    {
        GameManager.instance.ToggleTimeScale();
        GameManager.instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}
