using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjective : Entity
{
    public delegate void SpawnOnDamage(int amount);
    public SpawnOnDamage OnDamage;

    private EntitySpawner[] spawners;
    private int amountOfSpawners;
    private bool spawnTimer = false;
    public float spawnCooldown;

    private void Awake()
    {
        spawners = GetComponentsInChildren<EntitySpawner>();
        amountOfSpawners = spawners.Length; 
        foreach(EntitySpawner spawner in spawners)
        {
            spawner.objectiveSpawner = true;
        }
    }

    protected override void DamageEvent(Entity Attacker)
    {
        if(!spawnTimer)
        {
            foreach(EntitySpawner spawner in spawners)
            {

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
}