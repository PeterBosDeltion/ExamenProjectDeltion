using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(Movement), typeof(TriggerAbility))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public int playerNumber = 0; //0 == player one, 1 == 2 etc.
    public bool playerInitialized;
    private Player player;
    private Movement movement;
    private TriggerAbility triggerAbility;
    private Animator playerAnimator;
    public AudioSource walkingSource;
    public bool inTutorial;
    public bool tutorialAbilityInit;

    public List<Ability> abilities = new List<Ability>();
    public Ability ultimateAbility;
    public static bool deployingAbility;
    public Loadout loadout;
    public GameObject handParent;
    public Weapon currentWeapon;

    public Weapon currentPrimary;
    public Weapon currentSecondary;

    [HideInInspector]
    public InputManager myInputManager;
    public SkinnedMeshRenderer playerMeshRenderer;

    public bool canSwitch = true;

    public Rigidbody myRigidbody;

    //Assigning the Player scripts to the controller(There is no reason for the PlayerController to be anywhere else than on the Player so no need for a Gameobject reference)
    private void Awake()
    {
        myInputManager = GetComponent<InputManager>();
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        triggerAbility = GetComponent<TriggerAbility>();
        playerAnimator = GetComponentInChildren<Animator>();
        loadout = GetComponent<Loadout>();
        myRigidbody = GetComponent<Rigidbody>();
       

        if(!player || !movement || !triggerAbility)
        {
            Debug.LogError("Not all player scripts have been asigned to the targeted object");
        }

        //mySource = GetComponent<AudioSource>();
    }

    public void InitializeLoadout()
    {
        if (!inTutorial)
        {
                loadout.GenerateLoadout(LoudoutManager.instance.playerLoadouts[playerNumber]);
                currentPrimary = loadout.primary;
                currentSecondary = loadout.secondary;

                abilities = loadout.abilities;
                ultimateAbility = loadout.ultimateAbility;
        }
    }

    public void DisablePlayer()
    {
        myInputManager.abilityEvent -= TriggerAbility;
        myInputManager.MovingEvent -= Move;
        myInputManager.RotatingEvent -= Rotate;
        myInputManager.leftMouseButtonEvent -= Shoot;
        myInputManager.scrollEvent -= SwitchWeapon;
        myInputManager.LastWeaponEvent -= SwitchToLastWeapon;

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
        triggerAbility.enabled = false;
    }

    public void EnablePlayer()
    {
        myInputManager.abilityEvent += TriggerAbility;
        myInputManager.MovingEvent += Move;
        myInputManager.RotatingEvent += Rotate;
        myInputManager.leftMouseButtonEvent += Shoot;
        myInputManager.scrollEvent += SwitchWeapon;
        myInputManager.LastWeaponEvent += SwitchToLastWeapon;

        currentWeapon.canShoot = true;

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in rends)
        {
            if(!renderer.GetComponent<LineRenderer>())
            renderer.enabled = true;
        }

        movement.enabled = true;
        playerAnimator.enabled = true;
        loadout.enabled = true;
        GetComponent<Entity>().enabled = true;
        triggerAbility.enabled = true;
    }

    //Subscribe Input functions to their Input
    public void Start()
    {
        myInputManager.abilityEvent += TriggerAbility;
        myInputManager.MovingEvent += Move;
        myInputManager.RotatingEvent += Rotate;
        myInputManager.leftMouseButtonEvent += Shoot;
        myInputManager.scrollEvent += SwitchWeapon;
        myInputManager.LastWeaponEvent += SwitchToLastWeapon;

    }

    private void Update()
    {
        if(myInputManager.isMoving)
        {
            if(!walkingSource.isPlaying)
                walkingSource.Play();
        }
        else if(walkingSource.isPlaying)
        {
            walkingSource.Stop();
        }
    }
    
    public void Initialize()
    {
        GetComponentInChildren<Outline>().color = playerNumber;

        if (!inTutorial)
        {
            InitializeLoadout();
        }

        foreach (Ability ability in abilities)
        {
            Instantiate(ability, transform);
        }

        abilities.Clear();
        Ability newUlt = Instantiate(ultimateAbility, transform);
        ultimateAbility = newUlt;
        ultimateAbility.myPlayer = player;
        Ability[] abs = GetComponentsInChildren<Ability>();

        foreach (Ability a in abs)
        {
            a.myPlayer = player;
            a.myPlayerController = this;
            if(!a.ultimate)
                abilities.Add(a);
        }

        if (!inTutorial)
        {
            GameObject newPrimary = Instantiate(currentPrimary.gameObject, Vector3.zero, currentPrimary.gameObject.transform.rotation, handParent.transform);
            newPrimary.transform.localPosition = newPrimary.GetComponent<Weapon>().handPosition;
            newPrimary.transform.localEulerAngles = newPrimary.GetComponent<Weapon>().handRotation;
            newPrimary.GetComponent<Weapon>().myPlayer = player;
            newPrimary.GetComponent<Weapon>().myPlayerController = this;
            currentWeapon = newPrimary.GetComponent<Weapon>();
            currentPrimary = newPrimary.GetComponent<Weapon>();

            GameObject newSecondary = Instantiate(currentSecondary.gameObject, Vector3.zero, currentSecondary.transform.rotation, handParent.transform);
            newSecondary.transform.localPosition = newSecondary.GetComponent<Weapon>().handPosition;
            newSecondary.transform.localEulerAngles = newSecondary.GetComponent<Weapon>().handRotation;
            newSecondary.GetComponent<Weapon>().myPlayer = player;
            newSecondary.GetComponent<Weapon>().myPlayerController = this;
            currentSecondary = newSecondary.GetComponent<Weapon>();

            newSecondary.SetActive(false);
        }
     

        myInputManager.playerIndex = playerNumber;
        playerMeshRenderer.materials[4].color = GameManager.instance.darkPlayerColors[playerNumber];
        playerMeshRenderer.materials[5].color = GameManager.instance.playerColors[playerNumber];
        playerInitialized = true;
        myInputManager.Initialize();
        GameManager.instance.activePlayers.Add(this);
        if (!GameManager.instance.playersSpawned)
            GameManager.instance.playersSpawned = true;
    }

   

    private void SwitchToLastWeapon()
    {
        if(canSwitch)
            SwitchWeapon(0);
    }

    private void SwitchWeapon(float f)
    {
        if (canSwitch)
        {
            if (!myInputManager.holdingTab)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon.StopCoroutines();
                currentWeapon = (currentWeapon == currentPrimary) ? currentWeapon = currentSecondary : currentWeapon = currentPrimary;
                currentWeapon.ResetValues();
                currentWeapon.gameObject.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

    //Unsubscribe Input functions for safety
    public void OnDestroy()
    {
        myInputManager.abilityEvent -= TriggerAbility;
        myInputManager.MovingEvent -= Move;
        myInputManager.RotatingEvent -= Rotate;
        myInputManager.leftMouseButtonEvent -= Shoot;
        myInputManager.scrollEvent -= SwitchWeapon;
        myInputManager.LastWeaponEvent -= SwitchToLastWeapon;
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
    public bool GetIfDeath()
    {
        return player.death;
    }
    #endregion

    //Input related functions
    #region
    public void TriggerAbility(int value)
    {
        if(value == 4)//Ultimate
        {
            if (!ultimateAbility.onCooldown)
            {
                if(ultimateAbility.currentUltCharge >= 100)
                {
                    ultimateAbility.UseAbility();
                }
            }
        }
        else //Normal ability
        {
            if (!abilities[value].onCooldown)
            {
                if (inTutorial) //Prevent usage in tutorial before explanation
                {
                    if(tutorialAbilityInit)
                        abilities[value].UseAbility();
                }
                else //Normal gameplay
                {
                    abilities[value].UseAbility();
                }
            }
        }
    }

    public void Move(float xAxis, float yAxis)
    {
        movement.Move(xAxis, yAxis, myRigidbody);

        float diffrenceAngle = transform.rotation.eulerAngles.y - Camera.main.transform.rotation.y;
        Vector3 animatedAxis = Quaternion.Euler(0,diffrenceAngle,0) * new Vector3(xAxis, 0, yAxis);
        ManageAnimations(false, animatedAxis.x, animatedAxis.z);

        if (inTutorial && TutorialManager.instance.currentStep == 1)
            TutorialManager.playerMovedDelegate.Invoke();
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