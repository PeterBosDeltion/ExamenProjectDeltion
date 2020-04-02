using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudoutSelectableWindow : MonoBehaviour
{
    public GameObject selectAbleButtonPrefab;
    public GameObject scrollContent;
    public List<Ability> allAbilities = new List<Ability>();
    public List<Ability> allUltimates = new List<Ability>();
    public List<Weapon> allPrimaries = new List<Weapon>();
    public List<Weapon> allSecondaries = new List<Weapon>();
    public void OpenWindow(string type, int abilityIndex, LoudoutMainMenuButton menuButton)
    {
        switch (type)
        {
            case "Primary":
                foreach (var p in allPrimaries)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().Initialize(p, null, true, null, gameObject, menuButton);
                }
                break;
            case "Secondary":
                foreach (var s in allSecondaries)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().Initialize(s, null, false, null, gameObject, menuButton);
                }
                break;
            case "Ability":
                foreach (var a in allAbilities)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().Initialize(null, a, false, abilityIndex, gameObject, menuButton);
                }
                break;
            case "Ultimate":
                foreach (var u in allUltimates)
                {
                    //if () { } if it is unlocked, make available to select
                    GameObject button = Instantiate(selectAbleButtonPrefab, scrollContent.transform);
                    button.GetComponent<LoudoutSelectableButton>().Initialize(null, u, false, null, gameObject, menuButton);
                }
                break;
        }
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
