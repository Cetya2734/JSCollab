using System;
using UnityEngine;

public class EventBus
{
    private static EventBus instance;
    
    // Declare a static instance of EventBus accessible from anywhere

    public static EventBus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventBus();
            }

            return instance;
        }
    }
    
    public event Action onOpenInventory;

    public event Action onCloseInventory;

    public event Action<ItemData, GameObject> onPickUpItem;

    public event Action onGameplayPaused;

    public event Action onGameplayResumed;

    public event Action<ItemData> onItemUsed;
    

    // Method to invoke the onOpenInventory event
    public void OpenInventory()
    {
        onOpenInventory?.Invoke();
    }

    public void CloseInventory()
    {
        onCloseInventory?.Invoke();
    }

    // Method to invoke the onPickUpItem event, providing ItemData
    public void PickUpItem(ItemData itemData, GameObject equippedObject)
    {
        onPickUpItem?.Invoke(itemData, equippedObject);
    }

    public void PauseGameplay()
    {
        onGameplayPaused?.Invoke();
    }

    public void ResumeGameplay()
    {
        onGameplayResumed?.Invoke();
    }

    private void Awake()
    {
        instance = this;
    }

    public void UseItem(ItemData item)
    {
        onItemUsed?.Invoke(item);
    }
}