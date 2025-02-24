//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class LightSwitch : MonoBehaviour
//{
//    public Light lightSource; // Assign in Inspector
//    public TextMeshProUGUI interactionText; // Assign UI Text in Inspector
//    private bool isOn = false;
//    private bool playerNearby = false;

//    [Header("Flicker Settings")]
//    public float flickerDuration = 3f; // How long the flicker lasts
//    public float minFlickerTime = 0.05f; // Minimum flicker interval
//    public float maxFlickerTime = 0.3f;  // Maximum flicker interval
//    public float flickerIntensityMin = 0.5f; // Minimum light intensity
//    public float flickerIntensityMax = 2f; // Maximum light intensity

//    void Start()
//    {
//        if (lightSource != null)
//            lightSource.enabled = false; // Start with the light off

//        if (interactionText != null)
//            interactionText.gameObject.SetActive(false);
//    }

//    void Update()
//    {
//        if (playerNearby && Input.GetKeyDown(KeyCode.E))
//        {
//            ToggleLight();
//        }
//    }

//    void ToggleLight()
//    {
//        isOn = !isOn;
//        lightSource.enabled = isOn;

//        if (isOn)
//        {
//            StartCoroutine(FlickerLight());
//        }
//    }

//    IEnumerator FlickerLight()
//    {
//        float endTime = Time.time + flickerDuration;

//        while (Time.time < endTime)
//        {
//            lightSource.enabled = !lightSource.enabled; // Toggle on/off
//            lightSource.intensity = Random.Range(flickerIntensityMin, flickerIntensityMax); // Change brightness
//            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime)); // Random flicker speed
//        }

//        lightSource.enabled = true;
//        lightSource.intensity = 1f; // Reset intensity to normal after flickering
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            playerNearby = true;
//            if (interactionText != null)
//                interactionText.gameObject.SetActive(true);
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            playerNearby = false;
//            if (interactionText != null)
//                interactionText.gameObject.SetActive(false);
//        }
//    }
//}
