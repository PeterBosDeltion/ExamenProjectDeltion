using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Tooltip("The amount of spawners used when spawning a wave")]
    public int spawnerSpread;
    private Player player;
    private List<EntitySpawner> allAvailableSpawners = new List<EntitySpawner>();

    private List<EntitySpawner> closestSpawners = new List<EntitySpawner>();

    //Wave values
    public float EnemyScaleValue;

    //Enemy values
    public float healthModifier;
    public float damageModifier;

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

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        SetdifficultyVariables(GameManager.instance.difficulty);

        foreach(EntitySpawner spawner in GameObject.FindObjectsOfType<EntitySpawner>())
        {
            if(!spawner.objectiveSpawner)
            {
                allAvailableSpawners.Add(spawner);
            }
        }
    }

    private void SetdifficultyVariables(int difficulty)
    {
        switch(difficulty)
        {
            case 1:
                break;
            case 2:
                healthModifier = 1;
                damageModifier = 1;
                break;
            case 3:
                break;
        }
    }

    public void SpawnWave()
    {

    }

    private void GetNearbySpawners()
    {
        for (int i = 0; i < spawnerSpread; i++)
        {
            float distance = Mathf.Infinity;
            EntitySpawner closestSpawner = allAvailableSpawners[0];

            foreach(EntitySpawner spawner in allAvailableSpawners)
            {
                float newDistance = Vector3.Distance(spawner.gameObject.transform.position, player.transform.position);
                if (distance > newDistance)
                {
                    distance = newDistance;
                    closestSpawner = spawner;
                }
            }

            closestSpawners.Add(closestSpawner);
        }
    }
}