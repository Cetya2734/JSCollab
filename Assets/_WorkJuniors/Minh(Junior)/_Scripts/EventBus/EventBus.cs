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
    
    public event Action<ItemData, GameObject> onPickUpItem;

    public event Action onGameplayPaused;

    public event Action onGameplayResumed;

    public event Action<ItemData> onItemUsed;
    public event Action<string, int> OnObjectiveProgress; // EventName, ProgressValue
    public event Action<string, string, int> OnObjectiveCreated; // EventName, StatusText, MaxValue
    
    
    private void Awake()
    {
        instance = this;
    }
    
    #region Inventory
    // Method to invoke the onPickUpItem event, providing ItemData
    public void PickUpItem(ItemData itemData, GameObject equippedObject)
    {
        onPickUpItem?.Invoke(itemData, equippedObject);
    }
    
    public void UseItem(ItemData item)
    {
        onItemUsed?.Invoke(item);
    }
    
    #endregion

    #region Gameplay
    public void PauseGameplay()
    {
        onGameplayPaused?.Invoke();
    }

    public void ResumeGameplay()
    {
        onGameplayResumed?.Invoke();
    }

    #endregion
    
    
    #region Objective
    public void AddObjectiveProgress(string eventName, int value)
    {
        OnObjectiveProgress?.Invoke(eventName, value);
    }

    public void CreateObjective(string eventName, string statusText, int maxValue)
    {
        OnObjectiveCreated?.Invoke(eventName, statusText, maxValue);
    }
    #endregion
}