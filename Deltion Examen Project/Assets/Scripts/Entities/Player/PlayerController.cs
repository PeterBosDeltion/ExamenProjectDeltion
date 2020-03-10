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
    public Loadout loadout;
    public Weapon currentWeapon; //[PH]

    //Assigning the Player scripts to the controller(There is no reason for the PlayerController to be anywhere else than on the Player so no need for a Gameobject reference)
    private void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        triggerAbility = GetComponent<TriggerAbility>();
        playerAnimator = GetComponentInChildren<Animator>();
        loadout = GetComponent<Loadout>();

        if(!player || !movement || !triggerAbility)
        {
            Debug.LogError("Not all player scripts have been asigned to the targeted object");
        }
    }

    public void DisablePlayer()
    {
        InputManager.MovingEvent -= Move;
        InputManager.RotatingEvent -= Rotate;
        InputManager.leftMouseButtonEvent -= Shoot;
        InputManager.scrollEvent -= SwitchWeapon;

        currentWeapon.canShoot = false;

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in rends)
        {
            renderer.enabled = false;
        }

        movement.enabled = false;
        playerAnimator.enabled = false;
        loadout.enabled = false;
        GetComponent<Entity>().enabled = false;
    }

    public void EnablePlayer()
    {
        Debug.LogError("YES");
        InputManager.MovingEvent += Move;
        InputManager.RotatingEvent += Rotate;
        InputManager.leftMouseButtonEvent += Shoot;
        InputManager.scrollEvent += SwitchWeapon;

        currentWeapon.canShoot = true;

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in rends)
        {
            renderer.enabled = true;
        }

        movement.enabled = true;
        playerAnimator.enabled = true;
        loadout.enabled = true;
        GetComponent<Entity>().enabled = true;
    }

    //Subscribe Input functions to their Input
    public void Start()
    {
        InputManager.abilityEvent += TriggerAbility;
        InputManager.MovingEvent += Move;
        InputManager.RotatingEvent += Rotate;
        InputManager.leftMouseButtonEvent += Shoot;
        InputManager.scrollEvent += SwitchWeapon;
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

    private void SwitchWeapon(float f)
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon.StopAllCoroutines();
        currentWeapon = (currentWeapon == loadout.primary) ? currentWeapon = loadout.secondary : currentWeapon = loadout.primary;
        currentWeapon.ResetValues();
        currentWeapon.gameObject.SetActive(true);
    }

    //Unsubscribe Input functions for safety
    public void OnDestroy()
    {
        InputManager.abilityEvent -= TriggerAbility;
        InputManager.MovingEvent -= Move;
        InputManager.RotatingEvent -= Rotate;
        InputManager.leftMouseButtonEvent -= Shoot;
        InputManager.scrollEvent -= SwitchWeapon;
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