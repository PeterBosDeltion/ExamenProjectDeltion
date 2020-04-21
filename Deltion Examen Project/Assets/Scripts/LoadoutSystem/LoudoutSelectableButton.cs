using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoudoutSelectableButton : MonoBehaviour
{
    public int myPlayerNumber;
    public Image icon;
    public TextMeshProUGUI nameText;
    private Weapon myWeapon;
    private Ability myAbility;

    private bool primary;
    private bool ult;
    private int myAbilityIndex;
    public GameObject myWindow;
    private LoudoutMainMenuButton myButton;

    private PlayerLoadoutMenu myMenu;
    public Color disabledColor;
    public Color normalIconColor;

    public void WindowInitialize(Weapon w = null, Ability a = null, bool isPrimary = false, int? abilityIndex = null, GameObject window = null, LoudoutMainMenuButton button = null)
    {
        myWindow = window;
        myButton = button;
        if(w && a)
        {
            Debug.LogError("NotAllowed");
            return;
        }

        if(w && !a)
        {
            if (isPrimary)
            {
                primary = true;
            }
            else
            {
                primary = false;
            }

            icon.sprite = w.myWeapon.uiIcon;
            nameText.text = w.myWeapon.name;
            myWeapon = w;
        }
        else if(a && !w)
        {
            if (a.ultimate)
            {
                ult = true;
            }
            else
            {
                ult = false;
            }

            if (abilityIndex != null)
            {
                myAbilityIndex = (int)abilityIndex;
            }

            icon.sprite = a.uiIcon;
            nameText.text = a.name;

            myAbility = a;
        }
    }
    public void MenuInitialize(PlayerLoadoutMenu menu, int abilityIndex, int playerNumber, Weapon w = null, Ability a = null, bool isPrimary = false)
    {
        myMenu = menu;
        myPlayerNumber = playerNumber;
        if (w && a)
        {
            Debug.LogError("NotAllowed");
            return;
        }

        if (w && !a)
        {
            if (isPrimary)
            {
                primary = true;
            }
            else
            {
                primary = false;
            }

            icon.sprite = w.myWeapon.uiIcon;
            nameText.text = w.myWeapon.name;
            myWeapon = w;
        }
        else if (a && !w)
        {
            if (a.ultimate)
            {
                ult = true;
            }
            else
            {
                ult = false;
            }

            myAbilityIndex = abilityIndex;

            icon.sprite = a.uiIcon;
            nameText.text = a.name;

            myAbility = a;
        }

        icon.color = normalIconColor;
        nameText.enabled = true;
        GetComponent<Button>().interactable = true;
    }

    public void Clicked()
    {
        if (!myMenu)
        {
            if (myWeapon)
            {
                if (primary)
                {
                    LoudoutManager.instance.SetSavedLoadoutPrimary(myWeapon);
                }
                else
                {
                    LoudoutManager.instance.SetSavedLoadoutPrimary(myWeapon);
                }

            }
            else if (myAbility)
            {
                if (ult)
                {
                    LoudoutManager.instance.SetSavedLoadoutUltimate(myAbility);
                }
                else
                {
                    // LoudoutManager.instance.SetSavedLoadoutAbility(myAbility, myAbilityIndex);
                }
            }

            myButton.myImage.sprite = icon.sprite;

            Destroy(myWindow);
        }
        else
        {
            if (myWeapon)
            {
                if (primary)
                {
                    LoudoutManager.instance.SetPlayerLoadoutPrimary(myPlayerNumber, myWeapon);
                }
                else
                {
                    LoudoutManager.instance.SetPlayerLoadoutSecondary(myPlayerNumber,myWeapon);
                }

            }
            else if (myAbility)
            {
                if (ult)
                {
                    LoudoutManager.instance.SetPlayerLoadoutUltimate(myPlayerNumber, myAbility);
                    myMenu.CloseUltimateSelect();
                }
                else
                {
                    LoudoutManager.instance.SetPlayerLoadoutAbility(myPlayerNumber, myAbilityIndex, myAbility);
                    myMenu.CloseAbilitySelect();
                }
            }

            switch (myAbilityIndex)
            {
                case 0:
                    myMenu.abilityOneImg.sprite = icon.sprite;
                    break;
                case 1:
                    myMenu.abilityTwoImg.sprite = icon.sprite;
                    break;
                case 2:
                    myMenu.abilityThreeImg.sprite = icon.sprite;
                    break;
                case 3:
                    myMenu.abilityFourImg.sprite = icon.sprite;
                    break;
                case 5:
                    myMenu.ultImg.sprite = icon.sprite;
                    break;
            }
        }
       
    }

    public void DisabledButton()
    {
        GetComponent<Button>().interactable = false;
        icon.color = disabledColor;
        nameText.enabled = false;
    }
}
