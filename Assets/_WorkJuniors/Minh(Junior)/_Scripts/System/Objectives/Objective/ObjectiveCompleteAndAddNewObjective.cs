using System.Collections;
using System.Collections.Generic;
using Core.Gameplay.Objectives;
using UnityEngine;

public class ObjectiveCompleteAndAddNewObjective : MonoBehaviour
{

    [Header("Current Objective")]
    [SerializeField] private string statusText = "Complete the tutorial";
    [SerializeField] private string currentEventTrigger = ""; // Event name if progress is event-driven
    [SerializeField] private int maxValue = 1;
    
    [Header("Last Objective")]
    [SerializeField] private string lastEventTrigger = "TutorialTaskDone"; // Must match ObjectiveTrigger's event
    
    [SerializeField] private float delayDuration = 1f;
    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        // Verify the objective exists first
        if (GameManager.Instance.Objectives.Objectives.Exists(o => o.EventTrigger == lastEventTrigger))
        {
            GameManager.Instance.Objectives.AddProgress(lastEventTrigger, 1);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(NewObjectiveCreation());
        }
        else
        {
            Debug.LogError($"No objective found with event: {lastEventTrigger}");
        }
    }

    IEnumerator NewObjectiveCreation()
    {
        yield return new WaitForSeconds(delayDuration);

        // Create and add the objective
        var objective = new Objective(currentEventTrigger, statusText, maxValue);
        GameManager.Instance.Objectives.AddObjective(objective);
            
        Debug.Log("New objective created");
        // Complete immediately if no event trigger
        if (string.IsNullOrEmpty(currentEventTrigger))
            objective.AddProgress(maxValue);
    }
}
