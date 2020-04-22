using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerLoadoutMenu : MonoBehaviour
{
    public int playerNumber = 0; //0 == 1, 1 == 2 etc.

    public TMP_Dropdown primaryWeaponDropDown;
    public TMP_Dropdown secondaryWeaponDropDown;

    public GameObject abilityScrollObject;
    public GameObject abilityScrollContent;
    public GameObject ultimateScrollObject;
    public GameObject ultimateScrollContent;

    public Image abilityOneImg;
    public Image abilityTwoImg;
    public Image abilityThreeImg;
    public Image abilityFourImg;
    public Image ultImg;

    public GameObject loadoutSelectablePrefab;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        primaryWeaponDropDown.ClearOptions();
        secondaryWeaponDropDown.ClearOptions();

        List<TMP_Dropdown.OptionData> primaryOptions = new List<TMP_Dropdown.OptionData>();
        List<TMP_Dropdown.OptionData> secondaryOptions = new List<TMP_Dropdown.OptionData>();

        foreach (var pw in IDManager.instance.allPrimaryWeapons)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = pw.myWeapon.name;
            if (!primaryOptions.Contains(data))
                primaryOptions.Add(data);
        }
          
        foreach (var sw in IDManager.instance.allSecondaryWeapons)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = sw.myWeapon.name;
            if (!secondaryOptions.Contains(data))
                secondaryOptions.Add(data);
        }



        primaryWeaponDropDown.AddOptions(primaryOptions);
        secondaryWeaponDropDown.AddOptions(secondaryOptions);

    }

    public void SetPrimary(int i)
    {
        LoudoutManager.instance.SetPlayerLoadoutPrimary(playerNumber, IDManager.instance.GetPrimaryWeaponByID(i));
    }

    public void SetSecondary(int i)
    {
        LoudoutManager.instance.SetPlayerLoadoutSecondary(playerNumber, IDManager.instance.GetSecondaryWeaponByID(i));
    }

    public void OpenAbilitySelect(int abilityIndex)
    {
        abilityScrollObject.SetActive(true);
        foreach (Transform existingObject in abilityScrollContent.transform)
        {
            Destroy(existingObject.gameObject);
        }
        foreach (Ability ability in IDManager.instance.allAbilities)
        {
            LoadoutTemplate template = LoudoutManager.instance.playerLoadouts[playerNumber];
            Ability one = IDManager.instance.GetAbilityByID(template.abilityOneID);
            Ability two = IDManager.instance.GetAbilityByID(template.abilityTwoID);
            Ability three = IDManager.instance.GetAbilityByID(template.abilityThreeID);
            Ability four = IDManager.instance.GetAbilityByID(template.abilityFourID);

            GameObject buttonObj = Instantiate(loadoutSelectablePrefab, abilityScrollContent.transform);
            LoudoutSelectableButton button = buttonObj.GetComponent<LoudoutSelectableButton>();

            button.MenuInitialize(this, abilityIndex, playerNumber, null, ability, false);

            if (one == ability || two == ability || three == ability || four == ability)
            {
                button.DisabledButton();
            }
        }

    }

    public void OpenUltimateSelect()
    {
        ultimateScrollObject.SetActive(true);
        foreach (Transform existingObject in ultimateScrollContent.transform)
        {
            Destroy(existingObject.gameObject);
        }
        foreach (Ability ultimate in IDManager.instance.allUltimateAbilities)
        {
            GameObject buttonObj = Instantiate(loadoutSelectablePrefab, ultimateScrollContent.transform);
            LoudoutSelectableButton button = buttonObj.GetComponent<LoudoutSelectableButton>();

            button.MenuInitialize(this, 5, playerNumber, null, ultimate, false);
        }

    }

    public void CloseAbilitySelect()
    {
        abilityScrollObject.SetActive(false);
    }

    public void CloseUltimateSelect()
    {
        ultimateScrollObject.SetActive(false);
    }

}
