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

    public List<Ability> abilities = new List<Ability>();

    public Weapon primary; //[PH]

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
        InputManager.abilityEvent += TriggerAbility;
        InputManager.MovingEvent += Move;
        InputManager.RotatingEvent += Rotate;
        InputManager.leftMouseButtonEvent += Shoot;
        Initialize();
    }

    private void Initialize()
    {
        foreach (Ability ability in abilities)
        {
            Instantiate(ability, transform);
        }

        abilities.Clear();
        Ability[] abs = GetComponentsInChildren<Ability>();

        foreach (Ability a in abs)
        {
            a.myPlayer = player;
            abilities.Add(a);
        }
    }

    //Unsubscribe Input functions for safety
    public void OnDestroy()
    {
        InputManager.abilityEvent -= TriggerAbility;
        InputManager.MovingEvent -= Move;
        InputManager.RotatingEvent -= Rotate;
        InputManager.leftMouseButtonEvent -= Shoot;
    }

    //Player GetValue functions
    #region
    public float GetHp()
    {
        return player.GetHp();
    }
    public float GetTempHp()
    {
        return player.GetTempHp();
    }
    public float GetMaxHp()
    {
        return player.maxHp;
    }

    public float GetMaxTempHp()
    {
        return player.maxTempHp;
    }
    #endregion

    //Input related functions
    #region
    public void TriggerAbility(int value)
    {
        if (!abilities[value].onCooldown)
            abilities[value].UseAbility();
    }

    public void Move(float xAxis, float yAxis)
    {
        movement.Move(xAxis, yAxis);
        Quaternion diffrence = transform.rotation * Quaternion.Inverse(Camera.main.transform.rotation);
        Vector3 animatedAxis = diffrence * new Vector3(xAxis, 0, yAxis);
        ManageAnimations(false, -animatedAxis.x, animatedAxis.z);
    }

    public void Rotate(float xAxis, float zAxis)
    {
        movement.Rotate(xAxis, zAxis);
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
    #endregion
}