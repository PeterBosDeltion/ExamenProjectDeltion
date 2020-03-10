using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUltimate : Entity
{
    private float myDamage;
    private MechUltimateAbility myAbility;
    private Movement movement;
    private Animator anim;
    public void Initialize(float mechHp, float damage, MechUltimateAbility ability)
    {
        maxHp = mechHp;
        hp = maxHp;
        movement = GetComponent<Movement>();
        myAbility = ability;

        InputManager.MovingEvent += Move;
        InputManager.RotatingEvent += Rotate;
        EntityManager.instance.AddPlayerOrAbility(this);
        //InputManager.leftMouseButtonEvent += Shoot;

        anim = GetComponentInChildren<Animator>();
        myDamage = damage;
    }

    protected override void Death()
    {
        myAbility.MechDestroyed();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        InputManager.MovingEvent -= Move;
        InputManager.RotatingEvent -= Rotate;

        EntityManager.instance.RemovePlayerOrAbility(this);
    }

    public void Move(float xAxis, float yAxis)
    {
        movement.Move(xAxis, yAxis);
    }

    public void Rotate(float xAxis, float zAxis)
    {
        movement.Rotate(xAxis, zAxis);
    }
}
