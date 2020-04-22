using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileMenu : MonoBehaviour
{
    public GameObject profileUnlockablePrefab;
    public GameObject profileUnlockableContent;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (Weapon pw in IDManager.instance.allPrimaryWeapons)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().weaponUnlock = pw.myWeapon;
            element.GetComponent<ProfileUnlockUI>().unlocked = true;
            element.GetComponent<ProfileUnlockUI>().Initialize();
        }
        foreach (Weapon sw in IDManager.instance.allSecondaryWeapons)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().weaponUnlock = sw.myWeapon;
            element.GetComponent<ProfileUnlockUI>().unlocked = true;
            element.GetComponent<ProfileUnlockUI>().Initialize();
        }

        foreach (Ability a in IDManager.instance.allAbilities)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().abilityUnlockPrefab = a.gameObject;
            element.GetComponent<ProfileUnlockUI>().unlocked = true;
            element.GetComponent<ProfileUnlockUI>().Initialize();
        }

        foreach (Ability u in IDManager.instance.allUltimateAbilities)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().abilityUnlockPrefab = u.gameObject;
            element.GetComponent<ProfileUnlockUI>().unlocked = true;
            element.GetComponent<ProfileUnlockUI>().Initialize();
        }
    }
}
