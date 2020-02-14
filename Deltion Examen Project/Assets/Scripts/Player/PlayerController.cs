using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(Movement), typeof(TriggerAbility))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private Movement movement;
    private TriggerAbility triggerAbility;
    private Animator playerAnimator;

    //Assigning the Player scripts to the controller(There is no reason for the PlayerController to be anywhere else than on the Player so no need for a Gameobject reference)
    private void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        triggerAbility = GetComponent<TriggerAbility>();
        playerAnimator = GetComponentInChildren<Animator>();

        if(!player || !movement || !triggerAbility)
        {
            Debug.LogError("Not all player scripts have been asigned to the targeted object");
        }
    }

    //Subscribe Input functions to their Input
    public void Start()
    {
        InputManager.Instance.abilityEvent += TriggerAbility;
        InputManager.Instance.MovingEvent += Move;
        InputManager.Instance.RotatingEvent += Rotate;
        InputManager.Instance.leftMouseButtonEvent += Shoot;
    }

    //Unsubscribe Input functions for safety
    public void OnDestroy()
    {
        InputManager.Instance.abilityEvent -= TriggerAbility;
        InputManager.Instance.MovingEvent -= Move;
        InputManager.Instance.RotatingEvent -= Rotate;
        InputManager.Instance.leftMouseButtonEvent -= Shoot;
    }

    public void TriggerAbility(float value)
    {

    }

    public void Move(float xAxis, float yAxis)
    {
        movement.Move(xAxis, yAxis);
        ManageAnimations(false, xAxis, yAxis);
    }

    public void Rotate(float xAxis, float zAxis)
    {
        movement.Rotate(xAxis, zAxis);
        Debug.Log("Rotating");
    }

    public void Shoot()
    {
        ManageAnimations(true);
    }
    private void ManageAnimations(bool shot = false, float xAxis = 0, float yAxis = 0)
    {
        playerAnimator.SetFloat("PosX", xAxis);
        playerAnimator.SetFloat("PosY", yAxis);
        playerAnimator.SetBool("Shoot", shot);
    }
}