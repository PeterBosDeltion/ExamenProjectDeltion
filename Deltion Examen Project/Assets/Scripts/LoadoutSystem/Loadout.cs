using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadout : MonoBehaviour
{
    public Weapon primary;
    public Weapon secondary;

    public List<Ability> abilities = new List<Ability>();
    public Ability ultimateAbility;

    public void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            abilities.Add(null);
        }

        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if(LoudoutManager.instance.currentSelectedLoadout == this)
    //    {
    //        FindObjectOfType<PlayerController>().loadout = this;
    //        FindObjectOfType<PlayerController>().InitializeLoadout();
    //        Debug.LogWarning("Placeholder code");
    //    }
    //}
}
