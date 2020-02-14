using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    public Player myPlayer;

    public TextMeshProUGUI ultCharge;
    public Image weaponImage;

    public Image abilityOneCDImg;
    public Image abilityTwoCDImg;
    public Image abilityThreeCDImg;
    public Image abilityFourCDImg;

    public Image healthBar;

    //private bool waiting;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        InputManager.Instance.abilityEvent += AbilityUsed;
    }

    public void AbilityUsed(float f)
    {
            switch (f)
            {
                case 0:
                    if (abilityOneCDImg.fillAmount == 0)
                        abilityOneCDImg.fillAmount = 1;
                    break;
                case 1:
                    if (abilityTwoCDImg.fillAmount == 0)
                        abilityTwoCDImg.fillAmount = 1;
                    break;
                case 2:
                    if (abilityThreeCDImg.fillAmount == 0)
                        abilityThreeCDImg.fillAmount = 1;
                    break;
                case 3:
                    if (abilityFourCDImg.fillAmount == 0)
                        abilityFourCDImg.fillAmount = 1;
                    break;
                default:
                    break;
            }
    }


    private void Update()
    {
        if (myPlayer.abilities[0].onCooldown)
        {
            abilityOneCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[0].cooldownTime;
        }
        if (myPlayer.abilities[1].onCooldown)
        {
            abilityTwoCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[1].cooldownTime;
        }
        if (myPlayer.abilities[2].onCooldown)
        {
            abilityThreeCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[2].cooldownTime;
        }
        if (myPlayer.abilities[3].onCooldown)
        {
            abilityFourCDImg.fillAmount -= Time.deltaTime / myPlayer.abilities[3].cooldownTime;
        }

        weaponImage.fillAmount =  myPlayer.primary.magazineAmmo / myPlayer.primary.totalAmmo;
        healthBar.fillAmount = myPlayer.hp / myPlayer.maxHp;
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
