using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightSwitchs : MonoBehaviour
{
    public Light lightSource; // Assign in Inspector
    public TextMeshProUGUI interactionText; // Assign UI Text in Inspector
    private bool isOn = false;
    private bool playerNearby = false;
    private Coroutine flickerCoroutine;

    [Header("Flicker Settings")]
    public float flickerDuration = 3f; // How long the flicker lasts
    public float minFlickerTime = 0.05f; // Minimum flicker interval
    public float maxFlickerTime = 0.3f;  // Maximum flicker interval
    public float flickerIntensityMin = 0.5f; // Minimum light intensity
    public float flickerIntensityMax = 2f; // Maximum light intensity

    [Header("Switch States")]
    public GameObject switchOffState; // Assign "Off" state child object
    public GameObject switchOnState;  // Assign "On" state child object

    [Header("Audio Settings")]
    public AudioSource audioSource; // Assign in Inspector
    public AudioClip turnOnSound;   // Assign in Inspector
    public AudioClip turnOffSound;  // Assign in Inspector
    public AudioClip flickerSound;  // Assign flickering sound effect in Inspector

    void Start()
    {
        if (lightSource != null)
            lightSource.enabled = false; // Start with the light off

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);

        UpdateSwitchState(); // Ensure initial state is set properly
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;

        if (!isOn)
        {
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
                flickerCoroutine = null;
            }
            StopFlickerSound();
            lightSource.enabled = false; // Make sure the light turns off
            lightSource.intensity = 1f; // Reset intensity
        }

        lightSource.enabled = isOn;
        PlayToggleSound();
        UpdateSwitchState();

        if (isOn)
        {
            flickerCoroutine = StartCoroutine(FlickerLight());
        }
    }

    void PlayToggleSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop(); // Stop any ongoing sound before playing a new one
            audioSource.clip = isOn ? turnOnSound : turnOffSound;
            audioSource.Play();
        }
    }

    void PlayFlickerSound()
    {
        if (audioSource != null && flickerSound != null)
        {
            audioSource.Stop();
            audioSource.clip = flickerSound;
            audioSource.loop = true; // Enable looping for flickering sound
            audioSource.Play();
        }
    }

    void StopFlickerSound()
    {
        if (audioSource != null && audioSource.clip == flickerSound)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }

    void UpdateSwitchState()
    {
        if (switchOffState != null)
            switchOffState.SetActive(!isOn); // Enable "Off" when light is off

        if (switchOnState != null)
            switchOnState.SetActive(isOn); // Enable "On" when light is on
    }

    IEnumerator FlickerLight()
    {
        float endTime = Time.time + flickerDuration;

        PlayFlickerSound(); // Start flickering sound

        while (Time.time < endTime)
        {
            lightSource.enabled = !lightSource.enabled; // Toggle on/off
            lightSource.intensity = Random.Range(flickerIntensityMin, flickerIntensityMax); // Change brightness
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

            // If the light is turned off during flickering, stop immediately
            if (!isOn)
            {
                StopFlickerSound();
                lightSource.enabled = false;
                lightSource.intensity = 1f; // Reset intensity
                yield break;
            }
        }

        StopFlickerSound(); // Stop flicker sound after flickering ends
        lightSource.enabled = true;
        lightSource.intensity = 1f; // Reset intensity to normal after flickering
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (interactionText != null)
                interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }
}
