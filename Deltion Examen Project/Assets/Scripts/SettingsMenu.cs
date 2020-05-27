using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer masterMixer;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown screenmodeDropdown;
    public TMP_Dropdown difficultyDropdown;
    public TMP_Dropdown mkDropdown;
    public Slider volumeSlider;
    private Resolution[] resolutions;

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        int currentResIndex = 0;
        if(resolutions != null)
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = i;
                }

            }
        }
       

        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
      
        volumeSlider.value = GetMasterLevel();

        qualityDropdown.value = QualitySettings.GetQualityLevel();
      
        screenmodeDropdown.value = GameManager.instance.screenMode;

        if(difficultyDropdown)
            difficultyDropdown.value = GameManager.instance.difficulty;
        if(mkDropdown)
            mkDropdown.value = GameManager.instance.mouseKeyboardPlayer;
    }

    private float GetMasterLevel()
    {
        float value;
        bool result = masterMixer.GetFloat("volume", out value);
        if (result)
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }

    private void Initialize()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        int currentResIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }

        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetVolume(float volume)
    {
        masterMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
    }

    public void SetResolution(int i)
    {
        Resolution res = resolutions[i];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

    }

    public void SetScreenMode(int i)
    {
        switch (i) 
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                GameManager.instance.screenMode = 0;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                GameManager.instance.screenMode = 1;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                GameManager.instance.screenMode = 2;
                break;
        }

    }
}
