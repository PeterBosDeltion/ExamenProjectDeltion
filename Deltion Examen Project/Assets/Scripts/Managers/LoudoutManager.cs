using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudoutManager : MonoBehaviour
{
    public List<LoadoutTemplate> savedLoadouts = new List<LoadoutTemplate>();
    public List<LoadoutTemplate> playerLoadouts = new List<LoadoutTemplate>();
  

    private LoadoutTemplate selectedSavedLoadout;
    public static LoudoutManager instance;

    public GameObject mainCanvas;
    public GameObject loadoutSelectableWindow;
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
        Initialize();
    }
  
    private void Initialize()
    {
        LoadoutTemplate standardTemplate = new LoadoutTemplate();
        for (int i = 0; i < 4; i++)
        {
            playerLoadouts.Add(standardTemplate);
        }

        SetDefaultLoadout(0);
    }

    private void SetDefaultLoadout(int playerID)
    {
        SetPlayerLoadoutPrimary(playerID, IDManager.instance.GetWeaponByID(1));
        SetPlayerLoadoutSecondary(playerID, IDManager.instance.GetWeaponByID(0));

        SetPlayerLoadoutAbility(playerID, 0, IDManager.instance.GetAbilityByID(0));
        SetPlayerLoadoutAbility(playerID, 1, IDManager.instance.GetAbilityByID(1));
        SetPlayerLoadoutAbility(playerID, 2, IDManager.instance.GetAbilityByID(2));
        SetPlayerLoadoutAbility(playerID, 3, IDManager.instance.GetAbilityByID(3));

        SetPlayerLoadoutUltimate(playerID, IDManager.instance.GetUltimateAbilityByID(0));

    }

    public void OpenLoadoutSelectableWindow(string type, int abilityIndex, LoudoutMainMenuButton button)
    {
        GameObject window = Instantiate(loadoutSelectableWindow, mainCanvas.transform);
        window.GetComponent<LoudoutSelectableWindow>().OpenWindow(type, abilityIndex, button);
    }

    public void SelectLoadout(int index)
    {
            selectedSavedLoadout = savedLoadouts[index];
    }

    public void SetPlayerLoadoutPrimary(int playerIndex, Weapon primary)
    {
        playerLoadouts[playerIndex].primaryID = IDManager.instance.GetIDByWeapon(primary);
    }

    public void SetPlayerLoadoutSecondary(int playerIndex, Weapon secondary)
    {
        playerLoadouts[playerIndex].secondaryID = IDManager.instance.GetIDByWeapon(secondary);
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
        selectedSavedLoadout.primaryID = IDManager.instance.GetIDByWeapon(primary);
    }

    public void SetSavedLoadoutSecondary(int playerIndex, Weapon secondary)
    {
        selectedSavedLoadout.secondaryID = IDManager.instance.GetIDByWeapon(secondary);
    }
    public void SetSavedLoadoutAbility(int playerIndex, int abilityIndex, Ability ability)
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
    public void SetSavedLoadoutUltimate(int playerIndex, Ability ultimate)
    {
        selectedSavedLoadout.ultimateID = IDManager.instance.GetIDByUltimateAbility(ultimate);
    }
}
