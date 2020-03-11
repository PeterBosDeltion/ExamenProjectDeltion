using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudoutManager : MonoBehaviour
{
    public List<Loadout> savedLoadouts = new List<Loadout>();
    public Loadout currentSelectedLoadout;
    public static LoudoutManager instance;

    public GameObject mainCanvas;
    public GameObject loadoutSelectableWindow;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject holder = Instantiate(new GameObject());
            holder.name = "LoadoutHolder";
            holder.AddComponent<Loadout>();
            DontDestroyOnLoad(holder);
            savedLoadouts.Add(holder.GetComponent<Loadout>());
        }
        SelectLoadout(0);
    }
    public void SetLoadoutPrimary(Weapon newPrimary)
    {
        currentSelectedLoadout.primary = newPrimary;
    }
    public void SetLoadoutSecondary(Weapon newSecondary)
    {
        currentSelectedLoadout.secondary = newSecondary;
    }

    public void SetLoadoutAbility(Ability newAbility, int abilityIndex)
    {
        currentSelectedLoadout.abilities[abilityIndex] = newAbility;
    }

    public void SetLoadoutUltimate(Ability newUltimate)
    {
        currentSelectedLoadout.ultimateAbility = newUltimate;
    }

    public void OpenLoadoutSelectableWindow(string type, int abilityIndex, LoudoutMainMenuButton button)
    {
        GameObject window = Instantiate(loadoutSelectableWindow, mainCanvas.transform);
        window.GetComponent<LoudoutSelectableWindow>().OpenWindow(type, abilityIndex, button);
    }

    public void SelectLoadout(int index)
    {
        currentSelectedLoadout = savedLoadouts[index];
    }

}
