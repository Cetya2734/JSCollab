using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObjectiveTrigger : MonoBehaviour
{
    [Header("Objective Settings")]
    [SerializeField] private string lastEvent = "DoorUnlocked";
    [SerializeField] private int progressValue = 1; // Progress to add when triggered
    
    [SerializeField] private string newEvent = "DoorUnlocked";
    [SerializeField] private string statusText = "DoorUnlocked";


    private LockedDoor _lockedDoor;
    private bool _objectiveTriggered;

    private void Awake()
    {
        _lockedDoor = GetComponent<LockedDoor>();
        if (_lockedDoor == null)
        {
            Debug.LogError("LockedDoor component not found!", this);
            return;
        }
    }

    private void OnEnable()
    {
        // Subscribe to the door's interaction event
        _lockedDoor.OnDoorUnlocked += HandleDoorUnlocked;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        _lockedDoor.OnDoorUnlocked -= HandleDoorUnlocked;
    }

    private void HandleDoorUnlocked()
    {
        if (_objectiveTriggered) return;

        // Notify the objective system
        EventBus.Instance.AddObjectiveProgress(lastEvent, progressValue);
        _objectiveTriggered = true;
        
        EventBus.Instance.CreateObjective(newEvent, statusText,progressValue);
        
    }
    
}
