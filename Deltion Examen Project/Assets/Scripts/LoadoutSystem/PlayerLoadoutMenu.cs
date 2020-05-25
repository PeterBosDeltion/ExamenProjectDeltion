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
    public GameObject scrollBarAbilities;
    public GameObject scrollBarUltimates;

    public Image inputMethodImg;
    public Sprite controllerIcon;
    public Sprite mkIcon;
    private void OnEnable()
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
            if(pw.myWeapon.requiredLevel <= PlayerProfile.instance.level)
            {
                TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
                data.text = pw.myWeapon.name;
                if (!primaryOptions.Contains(data))
                    primaryOptions.Add(data);
            }
           
        }
          
        foreach (var sw in IDManager.instance.allSecondaryWeapons)
        {
            if (sw.myWeapon.requiredLevel <= PlayerProfile.instance.level)
            {
                TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
                data.text = sw.myWeapon.name;
                if (!secondaryOptions.Contains(data))
                    secondaryOptions.Add(data);
            }
        }



        primaryWeaponDropDown.AddOptions(primaryOptions);
        secondaryWeaponDropDown.AddOptions(secondaryOptions);

        primaryWeaponDropDown.value = LoudoutManager.instance.playerLoadouts[playerNumber].primaryID;
        secondaryWeaponDropDown.value = LoudoutManager.instance.playerLoadouts[playerNumber].secondaryID;

        abilityOneImg.sprite = IDManager.instance.GetAbilityByID(LoudoutManager.instance.playerLoadouts[playerNumber].abilityOneID).uiIcon;
        abilityTwoImg.sprite = IDManager.instance.GetAbilityByID(LoudoutManager.instance.playerLoadouts[playerNumber].abilityTwoID).uiIcon;
        abilityThreeImg.sprite = IDManager.instance.GetAbilityByID(LoudoutManager.instance.playerLoadouts[playerNumber].abilityThreeID).uiIcon;
        abilityFourImg.sprite = IDManager.instance.GetAbilityByID(LoudoutManager.instance.playerLoadouts[playerNumber].abilityFourID).uiIcon;

        ultImg.sprite = IDManager.instance.GetUltimateAbilityByID(LoudoutManager.instance.playerLoadouts[playerNumber].ultimateID).uiIcon;

        if(GameManager.instance.mouseKeyboardPlayer == playerNumber)
        {
            inputMethodImg.sprite = mkIcon;
        }
        else
        {
            inputMethodImg.sprite = controllerIcon;
        }

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
        FindObjectOfType<UIManager>().SetSelectedObject(scrollBarAbilities);
        abilityScrollObject.SetActive(true);
        foreach (Transform existingObject in abilityScrollContent.transform)
        {
            Destroy(existingObject.gameObject);
        }
        foreach (Ability ability in IDManager.instance.allAbilities)
        {
            if(ability.requiredLevel <= PlayerProfile.instance.level)
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

    }

    public void OpenUltimateSelect()
    {
        FindObjectOfType<UIManager>().SetSelectedObject(scrollBarUltimates);
        ultimateScrollObject.SetActive(true);
        foreach (Transform existingObject in ultimateScrollContent.transform)
        {
            Destroy(existingObject.gameObject);
        }
        foreach (Ability ultimate in IDManager.instance.allUltimateAbilities)
        {
            if (ultimate.requiredLevel <= PlayerProfile.instance.level)
            {
                GameObject buttonObj = Instantiate(loadoutSelectablePrefab, ultimateScrollContent.transform);
                LoudoutSelectableButton button = buttonObj.GetComponent<LoudoutSelectableButton>();

                button.MenuInitialize(this, 5, playerNumber, null, ultimate, false);
            }
        }

    }

    public void CloseAbilitySelect()
    {
        FindObjectOfType<UIManager>().SetSelectedObject(primaryWeaponDropDown.gameObject);
        abilityScrollObject.SetActive(false);
    }

    public void CloseUltimateSelect()
    {
        FindObjectOfType<UIManager>().SetSelectedObject(primaryWeaponDropDown.gameObject);
        ultimateScrollObject.SetActive(false);
    }

   
}
