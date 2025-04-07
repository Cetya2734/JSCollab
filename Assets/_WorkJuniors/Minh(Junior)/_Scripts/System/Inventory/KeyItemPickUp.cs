using System.Collections;
using System.Collections.Generic;
using Core.Gameplay.Objectives;
using UnityEngine;

public class KeyItemPickUp : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameObject equippedObject;
    [SerializeField] private GameObject pickUpText;
    [SerializeField] private GameObject EPrompt;

    [Header("Objective Settings")]
    [SerializeField] private string _completionEvent = "ItemPickedUp"; // Event to trigger
    [SerializeField] private int _progressValue = 1; // Progress amount

    public void Interact()
    {
        // Item pickup logic
        if (EventBus.Instance != null)
        {
            EventBus.Instance.PickUpItem(_itemData, equippedObject);
            EPrompt.SetActive(false);
            
            // Notify objective system
            if (!string.IsNullOrEmpty(_completionEvent))
            {
                GameManager.Instance?.Objectives.AddProgress(_completionEvent, _progressValue);
            }
        }
        else
        {
            Debug.LogError("EventBus instance is null!");
        }

        Destroy(gameObject);
    }

    public GameObject InteractionText() => pickUpText;

    void OnDestroy()
    {
        if (pickUpText != null && pickUpText.activeSelf)
        {
            pickUpText.SetActive(false);
        }
    }
}
