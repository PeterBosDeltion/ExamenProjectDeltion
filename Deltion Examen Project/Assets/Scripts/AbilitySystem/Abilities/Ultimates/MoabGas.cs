using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoabGas : MonoBehaviour
{
    private float myDmg;
    private Player myPlayer;

    private List<Enemy> enemies = new List<Enemy>();

    public void Initialize(float damage, Player player, float aoeRadius)
    {
        myDmg = damage;
        myPlayer = player;
        GetComponent<SphereCollider>().radius = aoeRadius;
        InvokeRepeating("DamagePulse", 0, 1);
     
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy e = other.GetComponent<Enemy>();
            if (!enemies.Contains(e))
            {
                enemies.Add(e);
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy e = other.GetComponent<Enemy>();
            if (enemies.Contains(e))
            {
                enemies.Remove(e);
            }
        }
    }
    private void DamagePulse()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(myDmg, myPlayer);
        }
    }


    private void OnDestroy()
    {
        CancelInvoke();
    }
}
