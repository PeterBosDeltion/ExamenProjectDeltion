using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnlockBlurbWindow : MonoBehaviour
{
    private WeaponScriptable weaponUnlock;
    private Ability abilityUnlock;

    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI normalText;
    public TextMeshProUGUI statText;

    public void Initialize(WeaponScriptable weapon = null, GameObject abilityPrefab = null)
    {
        if(!weapon && !abilityPrefab)
        {
            Debug.LogError("Please assign at least one unlock type");
            return;
        }
        else
        {
            if (weapon)
            {
                weaponUnlock = weapon;
                abilityUnlock = null;
            }
            if (abilityPrefab)
            {
                abilityUnlock = abilityPrefab.GetComponent<Ability>();
                weaponUnlock = null;
            }
            SetTexts();
        }
    }
    private void SetTexts()
    {
        if(weaponUnlock && abilityUnlock)
        {
            Debug.LogError("Unlock UI contains conflicting types, please only assign one type");
            return;
        }

        if(weaponUnlock && !abilityUnlock)
        {
            icon.sprite = weaponUnlock.uiIcon;
            nameText.text = weaponUnlock.name;
            statText.text = WeaponStatText();
            normalText.text = weaponUnlock.description;
        }
        else if(abilityUnlock && !weaponUnlock)
        {
            icon.sprite = abilityUnlock.uiIcon;
            nameText.text = abilityUnlock.name;
            statText.text = AbilityStatText();
            normalText.text = abilityUnlock.description;
        }
    }

    private string WeaponStatText()
    {
        string stats = "";

        stats += "Damage: " + weaponUnlock.damage + "\n";
        stats += "Firerate: " + weaponUnlock.firerate + "\n";
        stats += "Reload speed: " + weaponUnlock.reloadSpeed + " " + "seconds" + "\n";
        stats += "Magazine: " + weaponUnlock.totalAmmo + "\n";
        stats += "Min fall off: " + weaponUnlock.minFallOff + "\n";
        stats += "Max fall off: " + weaponUnlock.maxFallOff + "\n";
        stats += "Required level: " + weaponUnlock.requiredLevel + "\n";

        return stats;
    }
    private string AbilityStatText()
    {
        string stats = "";
        string isUlt = (abilityUnlock.ultimate) ? "Yes" : "No";
        stats += "Ultimate: " + isUlt + "\n";
        stats += "Cooldown: " + abilityUnlock.cooldownTime + "\n";
        stats += "Duration: " + abilityUnlock.duration + "\n";
        stats += "Required level: " + abilityUnlock.requiredLevel + "\n";

        return stats;
    }
    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
