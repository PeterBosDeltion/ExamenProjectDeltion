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

    //Assigning the Player scripts to the controller(There is no reason for the PlayerController to be anywhere else than on the Player so no need for a Gameobject reference)
    private void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        triggerAbility = GetComponent<TriggerAbility>();

        if(!player || !movement || !triggerAbility)
        {
            Debug.LogError("Not all player scripts have been asigned to the targeted object");
        }
    }

    //Subscribe Input functions to their Input
    public void Start()
    {
        InputManager.Instance.reloadEvent += Reload;
        InputManager.Instance.abilityEvent += TriggerAbility;
        InputManager.Instance.MovingEvent += Move;
    }

    //Unsubscribe Input functions for safety
    public void OnDestroy()
    {
        InputManager.Instance.reloadEvent -= Reload;
        InputManager.Instance.abilityEvent -= TriggerAbility;
        InputManager.Instance.MovingEvent -= Move;
    }

    public void Reload()
    {

    }

    public void TriggerAbility(float value)
    {

    }

    public void Move(float xAxis, float yAxis)
    {
        movement.Move(xAxis, yAxis);
    }
}
