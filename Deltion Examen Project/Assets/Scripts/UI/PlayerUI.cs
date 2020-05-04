using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    public PlayerController myPlayer;
    public int myPlayersNumber;
    public Color mainPlayerColor;
    public Color darkPlayerColor;
    public List<Image> playerMainColoredImages = new List<Image>();
    public List<Image> playerDarkColoredImages = new List<Image>();

    public Image weaponImage;
    public Image weaponBackImage;

    public Image abilityOneCDImg;
    public Image abilityTwoCDImg;
    public Image abilityThreeCDImg;
    public Image abilityFourCDImg;

    public TextMeshProUGUI abilityOneHotkeyText;
    public TextMeshProUGUI abilityTwoHotkeyText;
    public TextMeshProUGUI abilityThreeHotkeyText;
    public TextMeshProUGUI abilityFourHotkeyText;
    public GameObject needReloadText;

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

    private bool waiting;
    private bool reloading;
    private float time = 0;

    //private bool waiting;
    public void GetMyPlayer()
    {
        switch (myPlayersNumber)
        {
            case 0:
                if(GameManager.instance.playerOne)
                    myPlayer = GameManager.instance.playerOne;
                break;
            case 1:
                if(GameManager.instance.playerTwo)
                myPlayer = GameManager.instance.playerTwo;
                break;
            case 2:
                if(GameManager.instance.playerThree)
                myPlayer = GameManager.instance.playerThree;
                break;
            case 3:
                if(GameManager.instance.playerFour)
                myPlayer = GameManager.instance.playerFour;
                break;
        }

        if (!waiting && myPlayer)
            StartCoroutine(WaitForPlayerInit());
    }

    private IEnumerator WaitForPlayerInit()
    {
        waiting = true;
        yield return new WaitUntil(() => myPlayer.playerInitialized);
        Initialize();
        waiting = false;
    }

    private void Initialize()
    {
        InputManager.delayedAbilityEvent += AbilityUsed;
        InputManager.reloadEvent += ReloadEvent;
        InputManager.scrollEvent += SwapEvent;
        InputManager.LastWeaponEvent += LastSwapEvent;

        foreach (Image img in playerMainColoredImages)
        {
            img.color = mainPlayerColor;
        }

        foreach (Image img in playerDarkColoredImages)
        {
            img.color = darkPlayerColor;
        }

        ultCharge.color = mainPlayerColor;
        abilityOneImg.sprite = myPlayer.abilities[0].uiIcon;
        abilityTwoImg.sprite = myPlayer.abilities[1].uiIcon;
        abilityThreeImg.sprite = myPlayer.abilities[2].uiIcon;
        abilityFourImg.sprite = myPlayer.abilities[3].uiIcon;
        ultIcon.sprite = myPlayer.ultimateAbility.uiIcon;

        weaponBackImage.sprite = myPlayer.currentWeapon.myWeapon.uiIcon;
        weaponImage.sprite = myPlayer.currentWeapon.myWeapon.uiIcon;
    }

    private void OnDestroy()
    {
        InputManager.delayedAbilityEvent -= AbilityUsed;
        InputManager.reloadEvent -= ReloadEvent;
        InputManager.scrollEvent -= SwapEvent;
        InputManager.LastWeaponEvent -= LastSwapEvent;
    }

    public void AbilityUsed(int f)
    {
            switch (f)
            {
                case 0:
                    if (abilityOneCDImg.fillAmount == 0 && !myPlayer.abilities[0].returned)
                        abilityOneCDImg.fillAmount = 1;
                        abilityOneHotkeyText.enabled = false;
                break;
                case 1:
                    if (abilityTwoCDImg.fillAmount == 0 && !myPlayer.abilities[1].returned)
                        abilityTwoCDImg.fillAmount = 1;
                        abilityTwoHotkeyText.enabled = false;
                    break;
            case 2:
                    if (abilityThreeCDImg.fillAmount == 0 && !myPlayer.abilities[2].returned)
                        abilityThreeCDImg.fillAmount = 1;
                        abilityThreeHotkeyText.enabled = false;
                    break;
            case 3:
                    if (abilityFourCDImg.fillAmount == 0 && !myPlayer.abilities[3].returned)
                        abilityFourCDImg.fillAmount = 1;
                        abilityFourHotkeyText.enabled = false;
                    break;
            default:
                    break;
            }
    }


    private void Update()
    {
        if (myPlayer)
        {
            if (myPlayer.playerInitialized)
            {
                if (myPlayer.abilities.Count > 0)
                {
                    if (myPlayer.abilities[0] != null)
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

                    abilityOneHotkeyText.enabled = (myPlayer.abilities[0].onCooldown || myPlayer.abilities[0].active) ? false : true;
                    abilityTwoHotkeyText.enabled = (myPlayer.abilities[1].onCooldown || myPlayer.abilities[1].active) ? false : true;
                    abilityThreeHotkeyText.enabled = (myPlayer.abilities[2].onCooldown || myPlayer.abilities[2].active) ? false : true;
                    abilityFourHotkeyText.enabled = (myPlayer.abilities[3].onCooldown || myPlayer.abilities[3].active) ? false : true;

                }

                if (myPlayer.ultimateAbility != null)
                {
                    if (Mathf.RoundToInt(myPlayer.ultimateAbility.currentUltCharge) >= 100)
                    {
                        ultCharge.text = "F/5";
                    }
                    else
                    {
                        ultCharge.text = "" + Mathf.RoundToInt(myPlayer.ultimateAbility.currentUltCharge) + "%";
                    }
                    ultChargeFilledImage.fillAmount = myPlayer.ultimateAbility.currentUltCharge / 100;
                    if (myPlayer.ultimateAbility.currentUltCharge >= 100)
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

                if (weaponImage.sprite != myPlayer.currentWeapon.myWeapon.uiIcon)
                {
                    weaponImage.sprite = myPlayer.currentWeapon.myWeapon.uiIcon;
                }
                if (weaponBackImage.sprite != myPlayer.currentWeapon.myWeapon.uiIcon)
                {
                    weaponBackImage.sprite = myPlayer.currentWeapon.myWeapon.uiIcon;
                }
                if (!reloading)
                {
                    weaponImage.fillAmount = myPlayer.currentWeapon.magazineAmmo / myPlayer.currentWeapon.totalAmmo;
                }
                if (myPlayer.currentWeapon.magazineAmmo <= 0)
                {
                    if (!needReloadText.activeSelf)
                        needReloadText.SetActive(true);
                }
                else
                {
                    if (needReloadText.activeSelf)
                        needReloadText.SetActive(false);
                }
                healthBar.fillAmount = myPlayer.GetHp() / myPlayer.GetMaxHp();

                if (myPlayer.GetTempHp() > 0)
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

                if (reloading)
                {
                    AnimateReload(myPlayer.currentWeapon);
                }

            }
        }
    }

    private void ReloadEvent()
    {
        if (myPlayer.currentWeapon.magazineAmmo < myPlayer.currentWeapon.totalAmmo)
        {
            if (!reloading)
            {
                time = 0;
                reloading = true;
            }
        }
    }

    private void SwapEvent(float f)
    {
        if (reloading)
        {
            reloading = false;
            time = 0;
        }
    }

    private void LastSwapEvent()
    {
        if (reloading)
        {
            reloading = false;
            time = 0;
        }
    }

    public void AnimateReload(Weapon w)
    {
        time += Time.deltaTime;
        weaponImage.fillAmount = time / w.myWeapon.reloadSpeed;
        if(weaponImage.fillAmount >= 1)
        {
            reloading = false;
            weaponImage.fillAmount = 1;
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
