using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, ISelectHandler
{
    public ItemData itemData;

    public GameObject equippedObject;

    private InventoryViewController _viewController;

    private Image _spawnedItemSprite;

    public void OnSelect(BaseEventData eventData)
    {
        // Notify the InventoryViewController that this slot is selected
        _viewController.OnSlotSelected(this);
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }

    public void ClearItemData()
    {
        itemData = null;

        if (_spawnedItemSprite != null)
        {
            Destroy(_spawnedItemSprite.gameObject);
        }
    }
    private void OnEnable()
    {
        // Find and store a reference to the InventoryViewController
        _viewController = FindObjectOfType<InventoryViewController>();

        if (itemData == null) return;
        // Instantiate and display the item's sprite in the slot
        _spawnedItemSprite = Instantiate<Image>(itemData.Sprite, transform.position, Quaternion.identity, transform);
    }

    private void OnDisable()
    {
        if (_spawnedItemSprite != null)
        {
            Destroy(_spawnedItemSprite);
        }
    }
}
