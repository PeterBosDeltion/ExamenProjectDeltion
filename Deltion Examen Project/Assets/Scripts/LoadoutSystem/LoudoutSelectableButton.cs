using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoudoutSelectableButton : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    private Weapon myWeapon;
    private Ability myAbility;

    private bool primary;
    private bool ult;
    private int myAbilityIndex;
    public GameObject myWindow;
    private LoudoutMainMenuButton myButton;

    public void Initialize(Weapon w = null, Ability a = null, bool isPrimary = false, int? abilityIndex = null, GameObject window = null, LoudoutMainMenuButton button = null)
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

    public void Clicked()
    {
        if (myWeapon)
        {
            if (primary)
            {
                LoudoutManager.instance.SetLoadoutPrimary(myWeapon);
            }
            else
            {
                LoudoutManager.instance.SetLoadoutSecondary(myWeapon);
            }

        }
        else if (myAbility)
        {
            if (ult)
            {
                LoudoutManager.instance.SetLoadoutUltimate(myAbility);
            }
            else
            {
                LoudoutManager.instance.SetLoadoutAbility(myAbility, myAbilityIndex);
            }
        }

        myButton.myImage.sprite = icon.sprite;

        Destroy(myWindow);
    }
}
