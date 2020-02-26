using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected float speed;
    protected float damage;
    protected float attackRange;

    private void Start()
    {
        hp = maxHp;
    }
}