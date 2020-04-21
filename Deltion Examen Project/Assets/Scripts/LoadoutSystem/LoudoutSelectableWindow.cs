﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudoutSelectableWindow : MonoBehaviour
{
    public GameObject selectAbleButtonPrefab;
    public GameObject scrollContent;

    public void OpenWindow(string type, int abilityIndex, LoudoutMainMenuButton menuButton)
    {
        switch (type)
        {
            case "Primary":
                foreach (var p in IDManager.instance.allPrimaryWeapons)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().WindowInitialize(p, null, true, null, gameObject, menuButton);
                }
                break;
            case "Secondary":
                foreach (var s in IDManager.instance.allSecondaryWeapons)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().WindowInitialize(s, null, false, null, gameObject, menuButton);
                }
                break;
            case "Ability":
                foreach (var a in IDManager.instance.allAbilities)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().WindowInitialize(null, a, false, abilityIndex, gameObject, menuButton);
                }
                break;
            case "Ultimate":
                foreach (var u in IDManager.instance.allUltimateAbilities)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().WindowInitialize(null, u, false, null, gameObject, menuButton);
                }
                break;
        }
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
