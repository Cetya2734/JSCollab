using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBench : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 2f;
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI Elements")]
    public GameObject interactionPromptUI;
    public GameObject upgradeMenuUI;

    private Transform currentWeaponBench;
    private bool isInRange = false;

    void Start()
    {
        // Ensure the UI elements are initially hidden
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(false);
        }
        if (upgradeMenuUI != null)
        {
            upgradeMenuUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check if we are in range of a weapon bench and the interaction key is pressed
        if (isInRange && Input.GetKeyDown(interactionKey))
        {
            ToggleUpgradeMenu();
        }

        // Check if the upgrade menu is open and the Escape key is pressed
        if (upgradeMenuUI != null && upgradeMenuUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            upgradeMenuUI.SetActive(false);
            // You might want to re-enable player controls here
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is a weapon bench
        if (other.CompareTag("WeaponBench"))
        {
            isInRange = true;
            currentWeaponBench = other.transform;

            // Show the interaction prompt
            if (interactionPromptUI != null)
            {
                interactionPromptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the current weapon bench
        if (other.transform == currentWeaponBench)
        {
            isInRange = false;
            currentWeaponBench = null;

            // Hide the interaction prompt
            if (interactionPromptUI != null)
            {
                interactionPromptUI.SetActive(false);
            }

            // Ensure the upgrade menu is closed when leaving
            if (upgradeMenuUI != null)
            {
                upgradeMenuUI.SetActive(false);
                // You might want to re-enable player controls here
            }
        }
    }

    void ToggleUpgradeMenu()
    {
        // Toggle the visibility of the upgrade menu
        if (upgradeMenuUI != null)
        {
            upgradeMenuUI.SetActive(!upgradeMenuUI.activeSelf);

            // You might want to disable player controls when the menu is open
            // and re-enable them when it's closed.
        }
    }
}
