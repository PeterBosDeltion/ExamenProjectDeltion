using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileMenu : MonoBehaviour
{
    public GameObject profileUnlockablePrefab;
    public GameObject profileUnlockableContent;

    public GameObject currentWindow;

    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI bannerCurrentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI nextUnlockText;

    public TMP_InputField userNameText;
    public TextMeshProUGUI bannerUserNameText;

    public Image nextUnlockImage;
    public Image expBarFilledImage;
    public Image bannerExpBarFilledImage;
    public TextMeshProUGUI expbarText;
    public TextMeshProUGUI bannerExpbarText;
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.Backslash))
            {
                ExperienceManager.instance.AwardExp(500);
                UpdateUI();
            }
        }
       
    }

    public void ChangeUsername(string s)
    {
        PlayerProfile.instance.SetUserName(s);
        UpdateUI();
    }

    public void UpdateUI()
    {
            userNameText.text = PlayerProfile.instance.template.username;
            bannerUserNameText.text = PlayerProfile.instance.template.username;

            Color imgC = nextUnlockImage.color;
            nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 0);
            nextUnlockText.text = "";
            currentLevelText.text = "Level " + PlayerProfile.instance.level;
            bannerCurrentLevelText.text = "Level " + PlayerProfile.instance.level;
            if (PlayerProfile.instance.level < PlayerProfile.instance.levelExpDictionary.Count - 1)
                {
                float nextLevel = PlayerProfile.instance.level + 1;
                nextLevelText.text = "Level " + nextLevel;
                float currentExp = PlayerProfile.instance.template.xp;
                float nextExp = PlayerProfile.instance.levelExpDictionary[PlayerProfile.instance.level + 1];
                nextExp -= PlayerProfile.instance.levelExpDictionary[PlayerProfile.instance.level];
            //float currentLvLExp = currentExp - PlayerProfile.instance.levelExpDictionary[PlayerProfile.instance.level];
            float exp = 0;
                for (int i = 0; i < PlayerProfile.instance.level; i++)
                {
                    exp += i * 1000;
                    Debug.Log(exp);
                }

                float calculatedExp = currentExp - exp;

                expbarText.text = calculatedExp + "/" + nextExp;
                expBarFilledImage.fillAmount = calculatedExp / nextExp;

                bannerExpbarText.text = calculatedExp + "/" + nextExp;
                bannerExpBarFilledImage.fillAmount = calculatedExp / nextExp;

            }
            else
            {
                nextLevelText.text = "";
                expBarFilledImage.fillAmount = 1;
                expbarText.text = "";

                bannerExpbarText.text = "";
                bannerExpBarFilledImage.fillAmount = 1;
        }
           
            foreach (Weapon pw in IDManager.instance.allPrimaryWeapons)
            {
                if (pw.myWeapon.requiredLevel == PlayerProfile.instance.level + 1)
                {
                    nextUnlockImage.sprite = pw.myWeapon.uiIcon;
                    nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                    nextUnlockText.text = pw.myWeapon.name;
                }
            }
            foreach (Weapon sw in IDManager.instance.allSecondaryWeapons)
            {

                if (sw.myWeapon.requiredLevel == PlayerProfile.instance.level + 1)
                {
                    nextUnlockImage.sprite = sw.myWeapon.uiIcon;
                    nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                    nextUnlockText.text = sw.myWeapon.name;
                }
            }

            foreach (Ability a in IDManager.instance.allAbilities)
            {

                if (a.requiredLevel == PlayerProfile.instance.level + 1)
                {
                    nextUnlockImage.sprite = a.uiIcon;
                    nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                    nextUnlockText.text = a.name;
                }
            }

            foreach (Ability u in IDManager.instance.allUltimateAbilities)
            {
                if (u.requiredLevel == PlayerProfile.instance.level + 1)
                {
                    nextUnlockImage.sprite = u.uiIcon;
                    nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                    nextUnlockText.text = u.name;
                }
            }

            ProfileUnlockUI[] uis = profileUnlockableContent.transform.GetComponentsInChildren<ProfileUnlockUI>();
            foreach (var element in uis)
            {
                if (element.weaponUnlock && !element.abilityUnlockPrefab)
                {
                    if (PlayerProfile.instance.level >= element.weaponUnlock.requiredLevel)
                    {
                        element.unlocked = true;
                        element.lockImage.SetActive(false);

                    }

                }
                else if (element.abilityUnlockPrefab && !element.weaponUnlock)
                {
                    if (PlayerProfile.instance.level >= element.abilityUnlockPrefab.GetComponent<Ability>().requiredLevel)
                    {
                        element.unlocked = true;
                        element.lockImage.SetActive(false);

                    }

                }
            }
    }

    public void Initialize()
    {
        userNameText.text = PlayerProfile.instance.template.username;
        bannerUserNameText.text = PlayerProfile.instance.template.username;

        nextUnlockImage.sprite = null;
        Color imgC = nextUnlockImage.color;
        nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 0);
        nextUnlockText.text = "";
        currentLevelText.text = "Level " + PlayerProfile.instance.level;
        bannerCurrentLevelText.text = "Level " + PlayerProfile.instance.level;
        if (PlayerProfile.instance.level < PlayerProfile.instance.levelExpDictionary.Count -1)
        {
            float nextLevel = PlayerProfile.instance.level + 1;
            nextLevelText.text = "Level " + nextLevel;
            float currentExp = PlayerProfile.instance.template.xp;
            float nextExp = PlayerProfile.instance.levelExpDictionary[PlayerProfile.instance.level + 1];
            nextExp -= PlayerProfile.instance.levelExpDictionary[PlayerProfile.instance.level];

            // float currentLvLExp = currentExp - PlayerProfile.instance.levelExpDictionary[PlayerProfile.instance.level];
            float exp = 0;
            for (int i = 0; i < PlayerProfile.instance.level; i++)
            {
                exp += i * 1000;
                Debug.Log(exp);
            }

            float calculatedExp = currentExp - exp;

            expbarText.text = calculatedExp + "/" + nextExp;
            expBarFilledImage.fillAmount = calculatedExp / nextExp;

            bannerExpbarText.text = calculatedExp + "/" + nextExp;
            bannerExpBarFilledImage.fillAmount = calculatedExp / nextExp;
        }
        else
        {
            nextLevelText.text = "";
            expBarFilledImage.fillAmount = 1;
            expbarText.text = "";

            bannerExpbarText.text = "";
            bannerExpBarFilledImage.fillAmount = 1;

        }





        foreach (Weapon pw in IDManager.instance.allPrimaryWeapons)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().weaponUnlock = pw.myWeapon;
            element.GetComponent<ProfileUnlockUI>().menu = this;
            element.GetComponent<ProfileUnlockUI>().Initialize();

            if(pw.myWeapon.requiredLevel == PlayerProfile.instance.level + 1)
            {
                nextUnlockImage.sprite = pw.myWeapon.uiIcon;
                nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                nextUnlockText.text = pw.myWeapon.name;
            }
        }
        foreach (Weapon sw in IDManager.instance.allSecondaryWeapons)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().weaponUnlock = sw.myWeapon;
            element.GetComponent<ProfileUnlockUI>().menu = this;
            element.GetComponent<ProfileUnlockUI>().Initialize();
            if (sw.myWeapon.requiredLevel == PlayerProfile.instance.level + 1)
            {
                nextUnlockImage.sprite = sw.myWeapon.uiIcon;
                nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                nextUnlockText.text = sw.myWeapon.name;
            }
        }

        foreach (Ability a in IDManager.instance.allAbilities)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().abilityUnlockPrefab = a.gameObject;
            element.GetComponent<ProfileUnlockUI>().menu = this;
            element.GetComponent<ProfileUnlockUI>().Initialize();
            if (a.requiredLevel == PlayerProfile.instance.level + 1)
            {
                nextUnlockImage.sprite = a.uiIcon;
                nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                nextUnlockText.text = a.name;
            }
        }

        foreach (Ability u in IDManager.instance.allUltimateAbilities)
        {
            GameObject element = Instantiate(profileUnlockablePrefab, profileUnlockableContent.transform);
            element.GetComponent<ProfileUnlockUI>().abilityUnlockPrefab = u.gameObject;
            element.GetComponent<ProfileUnlockUI>().menu = this;
            element.GetComponent<ProfileUnlockUI>().Initialize();
            if (u.requiredLevel == PlayerProfile.instance.level + 1)
            {
                nextUnlockImage.sprite = u.uiIcon;
                nextUnlockImage.color = new Color(imgC.r, imgC.g, imgC.b, 1);
                nextUnlockText.text = u.name;
            }
        }
    }
}
