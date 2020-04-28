using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileUnlockUI : MonoBehaviour
{
    public bool unlocked;
    public GameObject lockImage;

    public WeaponScriptable weaponUnlock;
    public GameObject abilityUnlockPrefab;

    public GameObject blurbWindowPrefab;

    public TextMeshProUGUI nameText;
    public Image icon;

    public ProfileMenu menu;

    public void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (weaponUnlock && !abilityUnlockPrefab)
        {
            nameText.text = weaponUnlock.name;
            icon.sprite = weaponUnlock.uiIcon;
        }
        else if (abilityUnlockPrefab && !weaponUnlock)
        {
            nameText.text = abilityUnlockPrefab.GetComponent<Ability>().name;
            icon.sprite = abilityUnlockPrefab.GetComponent<Ability>().uiIcon;
        }

        if (unlocked)
        {
            lockImage.SetActive(false);
        }
    }

    public void Clicked()
    {
        if (menu.currentWindow)
            Destroy(menu.currentWindow);
        GameObject window = Instantiate(blurbWindowPrefab, GetComponentInParent<Canvas>().transform);
        window.GetComponent<UnlockBlurbWindow>().Initialize(weaponUnlock, abilityUnlockPrefab);

        menu.currentWindow = window;
    }
}
