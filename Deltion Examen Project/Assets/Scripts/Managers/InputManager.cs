using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This Manager is used as a centralized point for all of the Players Input.
//This makes it easier to find problems related to input for debugging purposes.

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    //Input Delegates. subscribe a function to trigger its contents on input.
    public delegate void BaseInput();
    public delegate void FloatInput(float value);
    public delegate void IntInput(int value);
    public delegate void AxisInput(float xAxis, float yAxis);

    public static BaseInput leftMouseButtonEvent;
    public static BaseInput leftMouseButtonHoldEvent;
    public static BaseInput leftMouseButtonUpEvent;
    public static BaseInput rightMouseButtonEvent;
    public static BaseInput reloadEvent;
    public static BaseInput interactEvent;
    public static BaseInput escapeEvent;
    public static BaseInput tabEvent;
    public static BaseInput tabUpEvent;
    public static BaseInput LastWeaponEvent;
    public static IntInput abilityEvent;
    public static IntInput delayedAbilityEvent;
    public static FloatInput scrollEvent;
    public static AxisInput MovingEvent;
    public static AxisInput RotatingEvent;

    public bool isMoving;

    //Rotation related values
    private RaycastHit hit;
    public LayerMask floor;

    private float timeHoldingInteract = 0;
    public bool holdingTab;

    //Asigning empty functions to the delegates to avoid Errors
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(transform.root.gameObject);
        }

        leftMouseButtonEvent += Empty;
        leftMouseButtonHoldEvent += Empty;
        leftMouseButtonUpEvent += Empty;
        rightMouseButtonEvent += Empty;
        reloadEvent += Empty;
        interactEvent += Empty;
        escapeEvent += Empty;
        LastWeaponEvent += Empty;
        abilityEvent += EmptyInt;
        delayedAbilityEvent += EmptyInt;
        scrollEvent += EmptyFloat;
        MovingEvent += EmptyAxis;
        RotatingEvent += EmptyAxis;
        tabEvent += Empty;
        tabUpEvent += Empty;
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void OnDestroy()
    {
        leftMouseButtonEvent -= Empty;
        leftMouseButtonHoldEvent -= Empty;
        leftMouseButtonUpEvent -= Empty;
        rightMouseButtonEvent -= Empty;
        reloadEvent -= Empty;
        interactEvent -= Empty;
        escapeEvent -= Empty;
        LastWeaponEvent -= Empty;
        abilityEvent -= EmptyInt;
        delayedAbilityEvent -= EmptyInt;
        scrollEvent -= EmptyFloat;
        MovingEvent -= EmptyAxis;
        RotatingEvent -= EmptyAxis;
        tabEvent -= Empty;
        tabUpEvent -= Empty;
    }

    private void Update()
    {
        //Generic input
        if(GameManager.instance.curentState == GameManager.GameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
                LeftMouse();
            if (Input.GetMouseButtonUp(0))
                LeftMouseUp();
            if (Input.GetMouseButton(0))
                LeftMouseHold();
            if (Input.GetMouseButtonDown(1))
                RightMouse();
            if (Input.GetButtonDown("Reload"))
                Reload();
            //if (Input.GetButtonDown("Interact"))
            //    Interact();
            if (Input.GetButton("Interact"))
                HoldInteract();
            if (Input.GetButtonUp("Interact"))
                ReleaseInteract();
            if (Input.GetButtonDown("Ability 01"))
                AbilityHotkeys(0);
            if (Input.GetButtonDown("Ability 02"))
                AbilityHotkeys(1);
            if (Input.GetButtonDown("Ability 03"))
                AbilityHotkeys(2);
            if (Input.GetButtonDown("Ability 04"))
                AbilityHotkeys(3);
            if (Input.GetButtonDown("Ability ult"))
                AbilityHotkeys(4);
            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
                SwitchWeapon(Input.GetAxis("Mouse ScrollWheel"));
            if (Input.GetButtonDown("Escape"))
                Escape();
            if (Input.GetButtonDown("LastWeapon"))
                LastWeapon();
            if (Input.GetButtonDown("Tab"))
                Tab();
            if (Input.GetButtonUp("Tab"))
                TabUp();
        }

    }

    //Fixed update for movementbased input to avoid physics problems
    private void FixedUpdate()
    {
        if(GameManager.instance.curentState == GameManager.GameState.Playing)
        {
            //Movement input
            if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Horizontal") < 0f || Input.GetAxis("Vertical") > 0f || Input.GetAxis("Vertical") < 0f)
                Moving(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            else
                isMoving = false;
            //Rotation input
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, floor))
                Rotating(hit.point.x, hit.point.z);
        }
    }

    //All functions related to the inputs
    #region
    //Generic input
    private void LeftMouse()
    {
        leftMouseButtonEvent.Invoke();
    }
    private void LeftMouseUp()
    {
        leftMouseButtonUpEvent.Invoke();
    }
    private void LeftMouseHold()
    {
        leftMouseButtonHoldEvent.Invoke();
    }
    private void RightMouse()
    {
        Debug.Log("Right mouse");
        rightMouseButtonEvent.Invoke();
    }
    private void Reload()
    {
        Debug.Log("Reload");
        reloadEvent.Invoke();
    }
    private void Interact()
    {
        Debug.Log("Interact");
        interactEvent.Invoke();
    }

    private void Tab()
    {
        holdingTab = true;
        tabEvent.Invoke();
    }
    private void TabUp()
    {
        holdingTab = false;
        tabUpEvent.Invoke();
    }

    private void HoldInteract()
    {
        timeHoldingInteract += Time.deltaTime;
    }

    private void ReleaseInteract()
    {
        timeHoldingInteract = 0;
    }

    public float GetTimeInteractHeld()
    {
        return timeHoldingInteract;
    }
    public void Escape()
    {
        escapeEvent.Invoke();
    }
    public void LastWeapon()
    {
        LastWeaponEvent.Invoke();
    }
    private void AbilityHotkeys(int inputAbility)
    {
        //Debug.Log(inputAbility);
        abilityEvent.Invoke(inputAbility);
    }
    private void SwitchWeapon(float inputScroll)
    {
        scrollEvent(inputScroll);
    }

    //Movement input
    private void Moving(float x, float y)
    {
        isMoving = true;
        MovingEvent.Invoke(x, y);
    }
    //Rotate input
    private void Rotating(float x, float y)
    {
        RotatingEvent.Invoke(x, y);
    }
    #endregion

    //Empty functions to avoid "nothing inside the delegate" Errors with the delegates
    #region
    public void Empty()
    {
    }
    public void EmptyFloat(float value)
    {
    }
    public void EmptyInt(int value)
    {
    }
    public void EmptyAxis(float xAxis, float yAxis)
    {
    }
    #endregion

    //Just incase a script doesnt properly wipe its delegate the InputManager will wipe the delegates when changing scene

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        ResetDelegates();
    }

    public void ResetDelegates()
    {
        leftMouseButtonEvent = null;
        leftMouseButtonHoldEvent = null;
        leftMouseButtonUpEvent = null;
        rightMouseButtonEvent = null;
        reloadEvent = null;
        interactEvent = null;
        escapeEvent = null;
        LastWeaponEvent = null;
        abilityEvent = null;
        delayedAbilityEvent = null;
        scrollEvent = null;
        MovingEvent = null;
        RotatingEvent = null;

        Awake();
    }
}