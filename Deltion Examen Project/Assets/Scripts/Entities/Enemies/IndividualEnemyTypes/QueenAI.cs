using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAI : MeleeEnemy
{
    [Space]
    [Tooltip("This multipleir will be aplied onto the enemies speed and damage")]
    public float BuffModifier;

    public List<Enemy> allBuffedEnemies = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>())
        {
            allBuffedEnemies.Add(other.GetComponent<Enemy>());
            other.GetComponent<Enemy>().SetTemporaryBuffValue(BuffModifier, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            allBuffedEnemies.Remove(other.GetComponent<Enemy>());
            other.GetComponent<Enemy>().SetTemporaryBuffValue(BuffModifier, true);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        foreach(Enemy enemy in allBuffedEnemies)
        {
            enemy.SetTemporaryBuffValue(BuffModifier, true);
        }
    }
}