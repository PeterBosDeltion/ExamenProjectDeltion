using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    protected float hp;
    protected float speed;
    protected float damage;
    protected float attackRange;

    public void TakeDamage(float takenDamage)
    {
        damage -= takenDamage;
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
