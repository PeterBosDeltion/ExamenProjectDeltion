using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;

    public List<Entity> AllPlayersAndAbilities = new List<Entity>();
    public List<Entity> AllEnemys = new List<Entity>();

    public float killedEnemies;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AddEnemy(Entity toAdd)
    {
        AllEnemys.Add(toAdd);
    }
    public void AddPlayerOrAbility(Entity toAdd)
    {
        AllPlayersAndAbilities.Add(toAdd);
    }

    public void RemoveEnemy(Entity toRemove)
    {
        AllEnemys.Remove(toRemove);
        killedEnemies++;
    }
    public void RemovePlayerOrAbility(Entity toRemove)
    {
        AllPlayersAndAbilities.Remove(toRemove);
    }

    public void RetargetPlayer()
    {
        foreach(Enemy enemy in AllEnemys)
        {
            enemy.myAI.UpdateDestination();
        }
    }

    private void WipeValues()
    {
        killedEnemies = 0;
        AllPlayersAndAbilities.Clear();
        AllEnemys.Clear();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        WipeValues();
    }
}
