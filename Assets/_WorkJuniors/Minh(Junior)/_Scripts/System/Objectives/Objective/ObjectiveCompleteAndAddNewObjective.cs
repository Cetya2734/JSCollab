using System.Collections;
using System.Collections.Generic;
using Core.Gameplay.Objectives;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveCompleteAndAddNewObjective : MonoBehaviour
{

    [Header("Current Objective")]
    [SerializeField] private string statusText = "Complete the tutorial";
    [SerializeField] private string currentEventTrigger = "";
    [SerializeField] private int maxValue = 1;
    
    [Header("Last Objective")]
    [SerializeField] private string lastEventTrigger = "TutorialTaskDone";
    
    [SerializeField] private float delayDuration = 1f;
    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {

        EventBus.Instance.AddObjectiveProgress(
            lastEventTrigger, 
            1 // Progress to complete
        );

        StartCoroutine(NewObjectiveCreation());
    }

    IEnumerator NewObjectiveCreation()
    {
        yield return new WaitForSeconds(delayDuration);
        EventBus.Instance.CreateObjective(
            currentEventTrigger, 
            statusText, 
            1 // Required progress
        );
    }
}
