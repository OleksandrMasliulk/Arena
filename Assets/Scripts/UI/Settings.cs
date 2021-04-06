using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown qualityLevelDropdown;
    public Toggle fullscreenModeToggle;

    public Slider musicVolumeSlider;
    public Slider soundsVolumeSlider;

    private Resolution[] resolutions;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        InitResolution();
        GetValues();
    }

    void InitResolution()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int resolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && Screen.fullScreen)
                resolution = i;
            else if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && !Screen.fullScreen)
                resolution = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolution;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    public void SetFullscreenMode(bool value)
    {
        Screen.fullScreen = value;
    }

    public void SetResolution(int index)
    {
        Resolution newResolution = resolutions[index];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
    }

    void GetValues()
    {
        //resolutionDropdown.value = Screen.currentResolution;
        qualityLevelDropdown.value = QualitySettings.GetQualityLevel();
        fullscreenModeToggle.isOn = Screen.fullScreen;

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", .5f);
        soundsVolumeSlider.value = PlayerPrefs.GetFloat("SoundsVolume", .5f);
    }

    public void SetSoundsVolume(float volume)
    {
        audioManager.SetSoundsVolume(volume);
        PlayerPrefs.SetFloat("SoundsVolume", soundsVolumeSlider.value);
    }

    public void SetMusicVolume(float volume)
    {
        audioManager.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }
}
