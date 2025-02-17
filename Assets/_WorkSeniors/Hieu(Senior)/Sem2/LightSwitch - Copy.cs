using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public Light lightSource; // Assign in Inspector
    public TextMeshProUGUI interactionText; // Assign UI Text in Inspector
    private bool isOn = false;
    private bool playerNearby = false;

    void Start()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false); // Hide text at start
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
        lightSource.enabled = isOn;

        if (isOn)
        {
            StartCoroutine(FlickerLight());
        }
    }

    IEnumerator FlickerLight()
    {
        float flickerDuration = 3f;
        float endTime = Time.time + flickerDuration;

        while (Time.time < endTime)
        {
            lightSource.enabled = !lightSource.enabled;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        lightSource.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (interactionText != null)
                interactionText.gameObject.SetActive(true); // Show text
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (interactionText != null)
                interactionText.gameObject.SetActive(false); // Hide text
        }
    }
}
