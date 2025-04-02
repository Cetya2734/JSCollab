using System.Collections;
using System.Collections.Generic;
using Core.Gameplay.Objectives;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveComplete : MonoBehaviour
{
    [SerializeField] private string eventTrigger = "TutorialTaskDone"; 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //EventBus.Instance.AddObjectiveProgress(eventTrigger, 1);

            // Verify the objective exists first
            if (GameManager.Instance.Objectives.Objectives.Exists(o => o.EventTrigger == eventTrigger))
            {
                GameManager.Instance.Objectives.AddProgress(eventTrigger, 1);
                Debug.Log($"Objective '{eventTrigger}' progressed!");
                GetComponent<Collider>().enabled = false;
            }
            else
            {
                Debug.LogError($"No objective found with event: {eventTrigger}");
            }
        }
    }
}
