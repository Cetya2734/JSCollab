using System.Collections;
using UnityEngine;

public class ObjectiveNote : MonoBehaviour, IInteractable
{
    [Header("Current Objective")]
    [SerializeField] private string lastEvent = "ReadNote";
    [SerializeField] private int progressValue = 1; // Progress to complete the current objective

    [Header("New Objective")]
    [SerializeField] private string newEvent = "NextObjective";
    [SerializeField] private string statusText = "Proceed to the next area";
    [SerializeField] private int maxValue = 1; // Required progress for the new objective

    [Header("Timing")]
    [SerializeField] private float delayDuration = 1f; // Delay before creating new objective

    [Header("UI")]
    [SerializeField] private GameObject notePanel; // Reference to the UI panel for the note
    
    private bool _objectiveTriggered;
    private bool _isNoteOpen;

    public void Interact()
    {
        
        // Show the note UI
        if (notePanel != null)
        {
            notePanel.SetActive(true);
            _isNoteOpen = true;
        }
        
        if (_objectiveTriggered) return;
        
        // Complete the current objective
        EventBus.Instance.AddObjectiveProgress(lastEvent, progressValue);
        _objectiveTriggered = true;

        // Start coroutine to create the new objective
        StartCoroutine(CreateNewObjective());
    }
    
    private void Update()
    {
        // Close the note with 'E' key if it's open
        if (_isNoteOpen && Input.GetKeyDown(KeyCode.E))
        {
            CloseNote();
        }
    }

    private void CloseNote()
    {
        if (notePanel != null)
        {
            notePanel.SetActive(false);
            _isNoteOpen = false;
        }
    }

    public GameObject InteractionText()
    {
        return null;
    }

    private IEnumerator CreateNewObjective()
    {
        yield return new WaitForSeconds(delayDuration);
        EventBus.Instance.CreateObjective(newEvent, statusText, maxValue);
    }
}