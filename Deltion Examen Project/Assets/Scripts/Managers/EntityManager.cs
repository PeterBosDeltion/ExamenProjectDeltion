using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;

    public List<Entity> AllPlayersAndAbilities = new List<Entity>();
    public List<Entity> AllEnemys = new List<Entity>();

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
}
