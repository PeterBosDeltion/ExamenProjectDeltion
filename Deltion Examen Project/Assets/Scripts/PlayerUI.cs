using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    public PlayerController myPlayer;

    public Image weaponImage;
    public Image weaponBackImage;

    public Image abilityOneCDImg;
    public Image abilityTwoCDImg;
    public Image abilityThreeCDImg;
    public Image abilityFourCDImg;

    public Image abilityOneImg;
    public Image abilityTwoImg;
    public Image abilityThreeImg;
    public Image abilityFourImg;

    //public Image abilityOneBGImg; todo: set these to player colors
    //public Image abilityTwoBGImg;
    //public Image abilityThreeBGImg;
    //public Image abilityFourBGImg;

    public TextMeshProUGUI ultCharge;
    public Image ultIcon;
    public Image ultChargeImage;
    public Image ultChargeFilledImage;


    public Image healthBar;
    public GameObject tempHealthbar;
    public Image tempHealthbarFilled;

    //private bool waiting;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        InputManager.delayedAbilityEvent += AbilityUsed;

        abilityOneImg.sprite = myPlayer.abilities[0].uiIcon;
        abilityTwoImg.sprite = myPlayer.abilities[1].uiIcon;
        abilityThreeImg.sprite = myPlayer.abilities[2].uiIcon;
        abilityFourImg.sprite = myPlayer.abilities[3].uiIcon;
        ultIcon.sprite = myPlayer.ultimateAbility.uiIcon;
    }

    public void AbilityUsed(int f)
    {
            switch (f)
            {
                case 0:
                    if (abilityOneCDImg.fillAmount == 0 && !myPlayer.abilities[0].returned)
                        abilityOneCDImg.fillAmount = 1;
                    break;
                case 1:
                    if (abilityTwoCDImg.fillAmount == 0 && !myPlayer.abilities[1].returned)
                        abilityTwoCDImg.fillAmount = 1;
                    break;
                case 2:
                    if (abilityThreeCDImg.fillAmount == 0 && !myPlayer.abilities[2].returned)
                        abilityThreeCDImg.fillAmount = 1;
                    break;
                case 3:
                    if (abilityFourCDImg.fillAmount == 0 && !myPlayer.abilities[3].returned)
                        abilityFourCDImg.fillAmount = 1;
                    break;
            default:
                    break;
            }
    }


    private void Update()
    {
        if(myPlayer.abilities.Count > 0)
        {
            if(myPlayer.abilities[0] != null)
            {
                if (myPlayer.abilities[0].onCooldown)
                {
                    abilityOneCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[0].cooldownTime;
                }
            }
            if (myPlayer.abilities[1] != null)
            {
                if (myPlayer.abilities[1].onCooldown)
                {
                    abilityTwoCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[1].cooldownTime;
                }
            }
            if (myPlayer.abilities[2] != null)
            {
                if (myPlayer.abilities[2].onCooldown)
                {
                    abilityThreeCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[2].cooldownTime;
                }
            }
            if (myPlayer.abilities[3] != null)
            {
                if (myPlayer.abilities[3].onCooldown)
                {
                    abilityFourCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[3].cooldownTime;
                }
            }
               
        }

        if(myPlayer.ultimateAbility != null)
        {
            ultCharge.text = "" + Mathf.RoundToInt(myPlayer.ultimateAbility.currentUltCharge) + "%";
            ultChargeFilledImage.fillAmount = myPlayer.ultimateAbility.currentUltCharge / 100;
            if(myPlayer.ultimateAbility.currentUltCharge >= 100)
            {
                ultChargeImage.enabled = false;
            }
            else
            {
                if (!ultChargeImage.enabled)
                {
                    ultChargeImage.enabled = true;
                }
            }
        }

        if(weaponImage.sprite != myPlayer.currentWeapon.myWeapon.uiIcon)
        {
            weaponImage.sprite = myPlayer.currentWeapon.myWeapon.uiIcon;
        }
        if (weaponBackImage.sprite != myPlayer.currentWeapon.myWeapon.uiIcon)
        {
            weaponBackImage.sprite = myPlayer.currentWeapon.myWeapon.uiIcon;
        }
        weaponImage.fillAmount =  myPlayer.currentWeapon.magazineAmmo / myPlayer.currentWeapon.totalAmmo;
        healthBar.fillAmount = myPlayer.GetHp() / myPlayer.GetMaxHp();

        if(myPlayer.GetTempHp() > 0)
        {
            if (!tempHealthbar.activeSelf)
            {
                tempHealthbar.SetActive(true);
            }

            tempHealthbarFilled.fillAmount = myPlayer.GetTempHp() / myPlayer.GetMaxTempHp();
        }
        else
        {
            if (tempHealthbar.activeSelf)
            {
                tempHealthbar.SetActive(false);
            }
        }
    }
    //private IEnumerator AbilityCooldown(Image abilityCDImg, float seconds)
    //{
    //    waiting = true;
    //    abilityCDImg.fillAmount = 1;

    //    //for (float t = 0; t <= seconds; t += Time.deltaTime)
    //    //{
    //    //        Debug.Log(t);
    //    //        abilityCDImg.fillAmount = lerped;
    //    //}
    //    for (float f = 0; f <= seconds; f += Time.deltaTime)
    //    {
    //    }
    //    yield return new WaitForSeconds(seconds);
    //    abilityCDImg.fillAmount = 0;
    //    waiting = false;
    //}

}
