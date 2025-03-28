using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;
    //public Slider mouseSensitivitySlider;
    public AudioMixer audioMixer;

   
    private List<Resolution> resolutions = new List<Resolution>();
    private int currentResolutionIndex = 0;

    private static float mouseSensitivity = 1.0f;

    private bool isActive = false;

    void Start()
    {

        LoadResolutions();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);



        //mouseSensitivitySlider.value = mouseSensitivity;
        //mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
        
        
        if (PlayerData.instance.isGameOver || isActive) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;

    }

    public void ToggleSettingsMenu()
    {
        isActive = !settingsPanel.activeSelf;
        settingsPanel.SetActive(isActive);

        if (isActive)
        {
            Time.timeScale = 0;
            Cursor.visible = true; 
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false; 
        }

    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("VolumeMaster", Mathf.Log10(volume) * 20);
    }


    void LoadResolutions()
    {
        resolutionDropdown.ClearOptions();
        Resolution[] availableResolutions = Screen.resolutions;
        List<string> options = new List<string>();

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            Resolution res = availableResolutions[i];
            string option = res.width + " x " + res.height;
            options.Add(option);
            resolutions.Add(res);

            if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    } 
    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }


    //public void SetMouseSensitivity(float sensitivity)
    //{
    //    mouseSensitivity = sensitivity;
    //}
}