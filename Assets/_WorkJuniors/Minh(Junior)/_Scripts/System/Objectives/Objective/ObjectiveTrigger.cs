using System;
using System.Collections;
using System.Collections.Generic;
using Core.Gameplay.Objectives;
using UnityEngine;

using UnityEngine;
using Core.Gameplay.Objectives;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private string statusText = "Complete the tutorial";
    [SerializeField] private string eventTrigger = ""; // Event name if progress is event-driven
    [SerializeField] private int maxValue = 1;

    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.CompareTag("Player"))
        {
            _triggered = true;
            EventBus.Instance.CreateObjective(
                eventTrigger, 
                statusText, 
                1 // Required progress
            );
            GetComponent<Collider>().enabled = false; // Disable trigger
        }
    }
}
