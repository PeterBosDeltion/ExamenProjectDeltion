using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoudoutManager : MonoBehaviour
{
    public List<LoadoutTemplate> savedLoadouts = new List<LoadoutTemplate>();
    public List<LoadoutTemplate> playerLoadouts = new List<LoadoutTemplate>();
  

    private LoadoutTemplate selectedSavedLoadout;
    public static LoudoutManager instance;

    public GameObject mainCanvas;
    public GameObject loadoutSelectableWindow;

    private bool generatedDefaults;
    private GameObject currentWindow;
    public bool loadoutsInit;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(transform.root);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        Initialize();
    }
  
    private void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            LoadoutTemplate standardTemplate = new LoadoutTemplate();
            if(playerLoadouts.Count < 4)
                playerLoadouts.Add(standardTemplate);
        }

        if (!generatedDefaults)
        {
            SetDefaultLoadout(0);
            SetDefaultLoadout(1);
            SetDefaultLoadout(2);
            SetDefaultLoadout(3);
            generatedDefaults = true;
        }

        loadoutsInit = true;
    }

    private void SetDefaultLoadout(int playerID)
    {
        SetPlayerLoadoutPrimary(playerID, IDManager.instance.GetPrimaryWeaponByID(0));
        SetPlayerLoadoutSecondary(playerID, IDManager.instance.GetSecondaryWeaponByID(0));

        SetPlayerLoadoutAbility(playerID, 0, IDManager.instance.GetAbilityByID(0));
        SetPlayerLoadoutAbility(playerID, 1, IDManager.instance.GetAbilityByID(1));
        SetPlayerLoadoutAbility(playerID, 2, IDManager.instance.GetAbilityByID(2));
        SetPlayerLoadoutAbility(playerID, 3, IDManager.instance.GetAbilityByID(3));

        SetPlayerLoadoutUltimate(playerID, IDManager.instance.GetUltimateAbilityByID(0));

    }

    public void OpenLoadoutSelectableWindow(string type, int abilityIndex, LoudoutMainMenuButton button)
    {
        if (currentWindow)
            Destroy(currentWindow);
        GameObject window = Instantiate(loadoutSelectableWindow, mainCanvas.transform);
        window.GetComponent<LoudoutSelectableWindow>().OpenWindow(type, abilityIndex, button);

        currentWindow = window;
    }

    public void SelectLoadout(int index)
    {
            selectedSavedLoadout = savedLoadouts[index];
    }

    public void SetPlayerLoadoutPrimary(int playerIndex, Weapon primary)
    {
        playerLoadouts[playerIndex].primaryID = IDManager.instance.GetIDByPrimaryWeapon(primary);
    }

    public void SetPlayerLoadoutSecondary(int playerIndex, Weapon secondary)
    {
        playerLoadouts[playerIndex].secondaryID = IDManager.instance.GetIDBySecondaryWeapon(secondary);
    }

    //Player loadouts
    public void SetPlayerLoadoutAbility(int playerIndex, int abilityIndex, Ability ability)
    {
        switch (abilityIndex)
        {
            case 0:
                playerLoadouts[playerIndex].abilityOneID = IDManager.instance.GetIDByAbility(ability);
                break;
            case 1:
                playerLoadouts[playerIndex].abilityTwoID = IDManager.instance.GetIDByAbility(ability);
                break;
            case 2:
                playerLoadouts[playerIndex].abilityThreeID = IDManager.instance.GetIDByAbility(ability);
                break;
            case 3:
                playerLoadouts[playerIndex].abilityFourID = IDManager.instance.GetIDByAbility(ability);
                break;
        }
    }

    public void SetPlayerLoadoutUltimate(int playerIndex, Ability ultimate)
    {
        playerLoadouts[playerIndex].ultimateID = IDManager.instance.GetIDByUltimateAbility(ultimate);
    }

    //Saved loadouts
    public void SetSavedLoadoutPrimary(Weapon primary)
    {
        selectedSavedLoadout.primaryID = IDManager.instance.GetIDByPrimaryWeapon(primary);
    }

    public void SetSavedLoadoutSecondary( Weapon secondary)
    {
        selectedSavedLoadout.secondaryID = IDManager.instance.GetIDBySecondaryWeapon(secondary);
    }
    public void SetSavedLoadoutAbility(int abilityIndex, Ability ability)
    {
        switch (abilityIndex)
        {
            case 0:
                selectedSavedLoadout.abilityOneID = IDManager.instance.GetIDByAbility(ability);
                break;
            case 1:
                selectedSavedLoadout.abilityTwoID = IDManager.instance.GetIDByAbility(ability);
                break;
            case 2:
                selectedSavedLoadout.abilityThreeID = IDManager.instance.GetIDByAbility(ability);
                break;
            case 3:
                selectedSavedLoadout.abilityFourID = IDManager.instance.GetIDByAbility(ability);
                break;
        }
    }
    public void SetSavedLoadoutUltimate(Ability ultimate)
    {
        selectedSavedLoadout.ultimateID = IDManager.instance.GetIDByUltimateAbility(ultimate);
    }
}
