using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager instance;

    public float xpGained;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AwardExp(float amount)
    {
        xpGained += amount;
        PlayerProfile.instance.RecieveExp(amount);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        xpGained = 0;
    }
}
