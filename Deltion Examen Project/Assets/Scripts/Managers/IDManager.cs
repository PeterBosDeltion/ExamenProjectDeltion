using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager : MonoBehaviour
{
    public static IDManager instance;
    public List<Weapon> allWeapons = new List<Weapon>();
    public List<Ability> allAbilities = new List<Ability>();
    public List<Ability> allUltimateAbilities = new List<Ability>();

    private Dictionary<int, Weapon> weaponIds = new Dictionary<int, Weapon>();
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
        int wI = -1;
        int aI = -1;
        int uI = -1;
        foreach (Weapon weapon in allWeapons)
        {
            wI++;
            weaponIds.Add(wI, weapon);
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

    public Weapon GetWeaponByID(int id)
    {
        return weaponIds[id];
    }
    public Ability GetAbilityByID(int id)
    {
        return abilityIds[id];
    }
    public Ability GetUltimateAbilityByID(int id)
    {
        return ultimateAbilityIds[id];
    }

    public int GetIDByWeapon(Weapon weapon)
    {
        foreach (var kvp in weaponIds)
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
