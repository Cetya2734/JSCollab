using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider voiceVolumeSlider;
    public Button backButton;
    public string mainMenuSceneName = "MainMenu";

    // Dictionary to store the original volume levels.
    private Dictionary<string, float> originalVolumes = new Dictionary<string, float>();

    void Start()
    {
        // Attach the listener to the slider
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        if (voiceVolumeSlider != null)
        {
            voiceVolumeSlider.onValueChanged.AddListener(SetVoiceVolume);
        }
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBackToMainMenu);
        }

        // Initialize the sliders and store the original volumes
        InitializeVolumeSlider("MasterVolume", masterVolumeSlider);
        InitializeVolumeSlider("MusicVolume", musicVolumeSlider);
        InitializeVolumeSlider("SFXVolume", sfxVolumeSlider);
        InitializeVolumeSlider("VoiceVolume", voiceVolumeSlider);
    }

    private void InitializeVolumeSlider(string volumeParam, Slider slider)
    {
        if (audioMixer != null && slider != null)
        {
            float volumeValue;
            bool result = audioMixer.GetFloat(volumeParam, out volumeValue);
            if (result)
            {
                // Convert the decibel value back to a 0-1 range (approximately).
                float normalizedVolume = Mathf.Pow(10, volumeValue / 20f);
                slider.value = normalizedVolume;
                originalVolumes[volumeParam] = volumeValue; // store original
            }
            else
            {
                Debug.LogError("Failed to get initial volume for parameter: " + volumeParam);
                slider.interactable = false; // Disable the slider if the parameter doesn't exist
            }
        }
        else
        {
            Debug.LogError("Audio Mixer or Slider is not assigned for parameter: " + volumeParam);
        }
    }

    void SetMasterVolume(float volume)
    {
        if (audioMixer != null)
        {
            // Convert the 0-1 range volume to a decibel value.  Important formula
            float dbValue = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("MasterVolume", dbValue);
        }
    }

    void SetMusicVolume(float volume)
    {
        if (audioMixer != null)
        {
            float dbValue = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("MusicVolume", dbValue);
        }
    }

    void SetSFXVolume(float volume)
    {
        if (audioMixer != null)
        {
            float dbValue = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("SFXVolume", dbValue);
        }
    }

    void SetVoiceVolume(float volume)
    {
        if (audioMixer != null)
        {
            float dbValue = Mathf.Log10(volume) * 20f;
            audioMixer.SetFloat("VoiceVolume", dbValue);
        }
    }

    void GoBackToMainMenu()
    {
        ResetVolumes();
        gameObject.SetActive(false);
    }

    private void ResetVolumes()
    {
        foreach (var entry in originalVolumes)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat(entry.Key, entry.Value);
            }
        }
        // reset the slider values to match.
        InitializeVolumeSlider("MasterVolume", masterVolumeSlider);
        InitializeVolumeSlider("MusicVolume", musicVolumeSlider);
        InitializeVolumeSlider("SFXVolume", sfxVolumeSlider);
        InitializeVolumeSlider("VoiceVolume", voiceVolumeSlider);
    }
}