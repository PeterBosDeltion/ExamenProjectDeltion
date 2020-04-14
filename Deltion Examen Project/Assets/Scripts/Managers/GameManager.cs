using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int difficulty = 2;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}