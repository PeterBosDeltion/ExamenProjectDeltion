using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadout : MonoBehaviour
{
    public Weapon primary;
    public Weapon secondary;

    public List<Ability> abilities = new List<Ability>();
    public Ability ultimateAbility;

    public void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            abilities.Add(null);
        }

    }

    public void GenerateLoadout(LoadoutTemplate template)
    {
        Weapon primary = IDManager.instance.GetPrimaryWeaponByID(template.primaryID);
        Weapon secondary = IDManager.instance.GetSecondaryWeaponByID(template.secondaryID);

        Ability aOne = IDManager.instance.GetAbilityByID(template.abilityOneID);
        Ability aTwo = IDManager.instance.GetAbilityByID(template.abilityTwoID);
        Ability aThree = IDManager.instance.GetAbilityByID(template.abilityThreeID);
        Ability aFour = IDManager.instance.GetAbilityByID(template.abilityFourID);

        Ability ult = IDManager.instance.GetUltimateAbilityByID(template.ultimateID);

        SetPrimary(primary);
        SetSecondary(secondary);

        SetAbility(0, aOne);
        SetAbility(1, aTwo);
        SetAbility(2, aThree);
        SetAbility(3, aFour);

        SetUltimate(ult);

    }

    public void SetPrimary(Weapon newPrimary)
    {
        primary = newPrimary;
    }
    public void SetSecondary(Weapon newSecondary)
    {
        secondary = newSecondary;
    }
    public void SetAbility(int index, Ability newAbility)
    {
        abilities[index] = newAbility;
    }
    public void SetUltimate(Ability newUltimate)
    {
        ultimateAbility = newUltimate;
    }
}
