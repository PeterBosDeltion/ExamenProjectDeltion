using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileUnlockUI : MonoBehaviour
{
    public bool unlocked;
    public GameObject lockImage;

    public WeaponScriptable weaponUnlock;
    public GameObject abilityUnlockPrefab;

    public GameObject blurbWindowPrefab;

    public void OnEnable()
    {
        if (unlocked)
        {
            lockImage.SetActive(false);
        }
    }

    public void Clicked()
    {
        GameObject window = Instantiate(blurbWindowPrefab, GetComponentInParent<Canvas>().transform);
        window.GetComponent<UnlockBlurbWindow>().Initialize(weaponUnlock, abilityUnlockPrefab);
    }
}
