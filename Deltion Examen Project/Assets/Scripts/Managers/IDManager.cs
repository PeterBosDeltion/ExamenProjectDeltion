using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager : MonoBehaviour
{
    public static IDManager instance;
    public List<Weapon> allPrimaryWeapons = new List<Weapon>();
    public List<Weapon> allSecondaryWeapons = new List<Weapon>();

    public List<Ability> allAbilities = new List<Ability>();
    public List<Ability> allUltimateAbilities = new List<Ability>();

    private Dictionary<int, Weapon> primaryWeaponIds = new Dictionary<int, Weapon>();
    private Dictionary<int, Weapon> secondaryWeaponIds = new Dictionary<int, Weapon>();
    private Dictionary<int, Ability> abilityIds = new Dictionary<int, Ability>();
    private Dictionary<int, Ability> ultimateAbilityIds = new Dictionary<int, Ability>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
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
        int pwI = -1;
        int swI = -1;
        int aI = -1;
        int uI = -1;
        foreach (Weapon primary in allPrimaryWeapons)
        {
            pwI++;
            primaryWeaponIds.Add(pwI, primary);
        }

        foreach (Weapon secondary in allSecondaryWeapons)
        {
            swI++;
            secondaryWeaponIds.Add(swI, secondary);
        }

        foreach (Ability ability in allAbilities)
        {
            aI++;
            abilityIds.Add(aI, ability);
        }
        foreach (Ability ult in allUltimateAbilities)
        {
            uI++;
            ultimateAbilityIds.Add(uI, ult);
        }
    }

    public Weapon GetPrimaryWeaponByID(int id)
    {
        return primaryWeaponIds[id];
    }
    public Weapon GetSecondaryWeaponByID(int id)
    {
        return secondaryWeaponIds[id];
    }
    public Ability GetAbilityByID(int id)
    {
        return abilityIds[id];
    }
    public Ability GetUltimateAbilityByID(int id)
    {
        return ultimateAbilityIds[id];
    }

    public int GetIDByPrimaryWeapon(Weapon weapon)
    {
        foreach (var kvp in primaryWeaponIds)
        {
            if (kvp.Value == weapon)
                return kvp.Key;
        }

        Debug.LogError("Weapon not found in database");
        return 0;
    }
    public int GetIDBySecondaryWeapon(Weapon weapon)
    {
        foreach (var kvp in secondaryWeaponIds)
        {
            if (kvp.Value == weapon)
                return kvp.Key;
        }

        Debug.LogError("Weapon not found in database");
        return 0;
    }
    public int GetIDByAbility(Ability ability)
    {
        foreach (var kvp in abilityIds)
        {
            if (kvp.Value == ability)
                return kvp.Key;
        }

        Debug.LogError("Ability not found in database");
        return 0;
    }
    public int GetIDByUltimateAbility(Ability ability)
    {
        foreach (var kvp in ultimateAbilityIds)
        {
            if (kvp.Value == ability)
                return kvp.Key;
        }

        Debug.LogError("Ultimate ability not found in database");
        return 0;
    }
}
