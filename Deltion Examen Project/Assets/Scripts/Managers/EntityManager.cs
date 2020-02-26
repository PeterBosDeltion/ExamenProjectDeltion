using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;

    public List<Entity> AllPlayersAndAbilitys = new List<Entity>();
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
        AllPlayersAndAbilitys.Add(toAdd);
    }
}
