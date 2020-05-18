using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePulse : MonoBehaviour
{
    private float myDamage;
    private Player myPlayer;
    private List<Enemy> hitEnemies = new List<Enemy>();
    public void Initialize(float damage, Player player)
    {
        myDamage = damage;
        myPlayer = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!hitEnemies.Contains(enemy))
            {
                enemy.TakeDamage(myDamage, myPlayer);
                if (enemy.GetHp() <= 0)
                    myPlayer.GetComponent<PlayerController>().ultimateAbility.IncrementUltCharge();
                hitEnemies.Add(enemy);
            }
        }
    }

    private void OnDestroy()
    {
        hitEnemies.Clear();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

}
