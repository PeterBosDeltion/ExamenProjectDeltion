using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjective : Entity
{
    public delegate void SpawnOnDamage(int amount);
    public SpawnOnDamage OnDamage;

    private EntitySpawner[] spawners;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public GameObject queen;
    public GameObject tank;
    bool queenSpawned;
    public bool shouldSpawnQueen = true; //False in tutorial

    private bool spawnTimer = false;
    public float spawnCooldown;

    [HideInInspector]
    public bool ObjectiveDone;

    protected virtual void Awake()
    {
        base.Awake();

        spawners = GetComponentsInChildren<EntitySpawner>();
        foreach(EntitySpawner spawner in spawners)
        {
            spawner.objectiveSpawner = true;
        }
    }

    protected override void DamageEvent(Entity Attacker)
    {
        if(!spawnTimer)
        {
            spawnTimer = true;

            int spawnsPerSpawner = Mathf.CeilToInt((float)enemiesToSpawn.Count / spawners.Length - 1);
            if (spawnsPerSpawner == 0)
                spawnsPerSpawner = 1;

            List<GameObject> wave = new List<GameObject>();
            wave.AddRange(enemiesToSpawn);

            if(!queenSpawned)
            {
                if (shouldSpawnQueen)
                {
                    spawners[0].AddToSpawnQue(queen);
                    queenSpawned = true;
                }
               
            }
            else
            {
                spawners[0].AddToSpawnQue(tank);
            }

            foreach (EntitySpawner spawner in spawners)
            {
                if(spawner != spawners[0])
                {
                    if(wave.Count != 0)
                    {
                        for (int i = 0; i < spawnsPerSpawner; i++)
                        {
                            int randomEntity = Random.Range(0, wave.Count - 1);
                            spawner.AddToSpawnQue(wave[randomEntity]);
                            wave.Remove(wave[randomEntity]);
                        }
                    }
                }
            }
            StartCoroutine(SpawnerCooldown());
        }
    }

    public override void SetEntityValues()
    {
        maxHp *= LevelManager.instance.objectiveHealthModifier;

        base.SetEntityValues();
    }

    public IEnumerator SpawnerCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
        spawnTimer = false;
    }

    protected override void Death()
    {
        base.Death();

        ObjectiveDone = true;
        LevelManager.instance.CheckObjectives();

        //Temporary
        Destroy(this.gameObject);
    }
}