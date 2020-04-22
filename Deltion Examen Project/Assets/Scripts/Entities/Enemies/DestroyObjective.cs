using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjective : Entity
{
    public delegate void SpawnOnDamage(int amount);
    public SpawnOnDamage OnDamage;

    private EntitySpawner[] spawners;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

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
            int spawnsPerSpawner = Mathf.CeilToInt((float)enemiesToSpawn.Count / spawners.Length);
            List<GameObject> wave = new List<GameObject>();
            wave.AddRange(enemiesToSpawn);

            foreach (EntitySpawner spawner in spawners)
            {
                if(wave.Count != 0)
                {
                    for (int i = 0; i < spawnsPerSpawner; i++)
                    {
                        int randomEntity = Random.Range(0, enemiesToSpawn.Count - 1);
                        spawner.AddToSpawnQue(wave[randomEntity]);
                        wave.Remove(wave[randomEntity]);
                    }
                }
            }
            StartCoroutine(SpawnerCooldown());
        }
    }

    public IEnumerator SpawnerCooldown()
    {
        spawnTimer = true;
        yield return new WaitForSeconds(spawnCooldown);
        spawnTimer = false;
    }

    protected override void Death()
    {
        ObjectiveDone = true;
        LevelManager.instance.CheckObjectives();
    }
}