using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioClip testSFX;

    Resolution[] resolutions;
    FullScreenMode[] fullscreenModes;

    public TMPro.TMP_Dropdown resDropdown;
    public TMPro.TMP_Dropdown fsmDropdown;

    private bool playSFX = true;
    private bool isFull = true;

    private void OnEnable()
    {
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        int it = 0;

        foreach (Resolution res in resolutions)
        {
            string option = res.width + " x " + res.height;
            options.Add(option);

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = it;
            }

            it++;
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];

        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public void ToggleVSync(bool value)
    {
        if (value)
            QualitySettings.vSyncCount = 0;
        else
            QualitySettings.vSyncCount = 1;
    }

    public void SetFullscreen(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                isFull = true;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                isFull = false;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                isFull = false;
                break;
        }
    }

    public void SetMusicVolume(float value)
    {
        if (value > -50)
            AudioManager.current.SetMusicVolume(value);
        else
            AudioManager.current.SetMusicVolume(-80);
    }

    public void SetSFXVolume(float value)
    {
        if (value > -20)
            AudioManager.current.SetSFXVolume(value);
        else
            AudioManager.current.SetSFXVolume(-80);

        if (!playSFX)
            return;

        playSFX = false;
        AudioManager.current.PlaySFX(testSFX);
        StartCoroutine(SFXDelay());
    }

    private IEnumerator SFXDelay()
    {
        yield return new WaitForSeconds(0.5f);

        playSFX = true;
    }
}
