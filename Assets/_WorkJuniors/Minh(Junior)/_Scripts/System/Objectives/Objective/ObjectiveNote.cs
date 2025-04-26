// using System.Collections;
// using UnityEngine;
//
// public class ObjectiveNote : MonoBehaviour, IInteractable
// {
//     [Header("Current Objective")]
//     [SerializeField] private string lastEvent = "ReadNote";
//     [SerializeField] private int progressValue = 1; // Progress to complete the current objective
//
//     [Header("New Objective")]
//     [SerializeField] private string newEvent = "NextObjective";
//     [SerializeField] private string statusText = "Proceed to the next area";
//     [SerializeField] private int maxValue = 1; 
//
//     [Header("Timing")]
//     [SerializeField] private float delayDuration = 1f; // Delay before creating new objective
//
//     [Header("UI")]
//     [SerializeField] private GameObject notePanel; // Reference to the UI panel for the note
//     
//     private bool _objectiveTriggered;
//     private bool _isNoteOpen;
//
//     public void Interact()
//     {
//         // Show the note UI
//         if (notePanel != null)
//         {
//             notePanel.SetActive(true);
//             _isNoteOpen = true;
//         }
//         
//         if (_objectiveTriggered) return;
//
//         // Complete the current objective
//         EventBus.Instance.AddObjectiveProgress(lastEvent, progressValue);
//         _objectiveTriggered = true;
//
//         // Start coroutine to create the new objective
//         StartCoroutine(CreateNewObjective());
//     }
//     
//     private void Update()
//     {
//         // Close the note with 'E' key if it's open
//         if (_isNoteOpen && Input.GetKeyDown(KeyCode.E))
//         {
//             CloseNote();
//         }
//     }
//
//     private void CloseNote()
//     {
//         if (notePanel != null)
//         {
//             notePanel.SetActive(false);
//             _isNoteOpen = false;
//         }
//     }
//
//     public GameObject InteractionText()
//     {
//         return null;
//     }
//
//     private IEnumerator CreateNewObjective()
//     {
//         yield return new WaitForSeconds(delayDuration);
//         EventBus.Instance.CreateObjective(newEvent, statusText, maxValue);
//     }
// }

// using System.Collections;
// using UnityEngine;
//
// public class ObjectiveNote : MonoBehaviour, IInteractable
// {
//     [Header("Current Objective")]
//     [SerializeField] private string lastEvent = "ReadNote";
//     [SerializeField] private int progressValue = 1; // Progress to complete the current objective
//
//     [Header("New Objective")]
//     [SerializeField] private string newEvent = "NextObjective";
//     [SerializeField] private string statusText = "Proceed to the next area";
//     [SerializeField] private int maxValue = 1;
//
//     [Header("Timing")]
//     [SerializeField] private float delayDuration = 1f; // Delay before creating new objective
//     [SerializeField] private float fadeDuration = 0.5f; // Duration for fade in/out
//
//     [Header("UI")]
//     [SerializeField] private CanvasGroup notePanelCanvasGroup; // Reference to the CanvasGroup on the note panel
//
//     private bool _objectiveTriggered;
//     private bool _isNoteOpen;
//
//     public void Interact()
//     {
//         if (_objectiveTriggered) return;
//
//         // Show the note UI by fading in
//         if (notePanelCanvasGroup != null)
//         {
//             StartCoroutine(FadeCanvasGroup(notePanelCanvasGroup, 1f, fadeDuration));
//             _isNoteOpen = true;
//         }
//         else
//         {
//             Debug.LogError("NotePanel CanvasGroup is not assigned!");
//         }
//
//         // Complete the current objective
//         EventBus.Instance.AddObjectiveProgress(lastEvent, progressValue);
//         _objectiveTriggered = true;
//
//         // Start coroutine to create the new objective
//         StartCoroutine(CreateNewObjective());
//     }
//
//     private void Update()
//     {
//         // Close the note with 'E' key if it's open
//         if (_isNoteOpen && Input.GetKeyDown(KeyCode.E))
//         {
//             CloseNote();
//         }
//     }
//
//     private void CloseNote()
//     {
//         if (notePanelCanvasGroup != null)
//         {
//             StartCoroutine(FadeCanvasGroup(notePanelCanvasGroup, 0f, fadeDuration));
//             _isNoteOpen = false;
//         }
//     }
//
//     public GameObject InteractionText()
//     {
//         return null;
//     }
//
//     private IEnumerator CreateNewObjective()
//     {
//         yield return new WaitForSeconds(delayDuration);
//         EventBus.Instance.CreateObjective(newEvent, statusText, maxValue);
//     }
//
//     private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
//     {
//         float startAlpha = canvasGroup.alpha;
//         float time = 0;
//
//         while (time < duration)
//         {
//             time += Time.deltaTime;
//             canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
//             yield return null;
//         }
//
//         canvasGroup.alpha = targetAlpha;
//     }
// }


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
    [SerializeField] private int maxValue = 1;

    [Header("Timing")]
    [SerializeField] private float delayDuration = 1f; // Delay before creating new objective
    [SerializeField] private float fadeDuration = 0.5f; // Duration for fade in/out

    [Header("UI")]
    [SerializeField] private CanvasGroup notePanelCanvasGroup; // Reference to the CanvasGroup on the note panel

    private bool _objectiveTriggered;
    private bool _isNoteOpen;
    private bool _isFading; // Prevent multiple fade coroutines

    public void Interact()
    {

        // Show the note UI by fading in
        if (notePanelCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(notePanelCanvasGroup, 1f, fadeDuration));
            _isNoteOpen = true;
        }
        else
        {
        }
        if (_objectiveTriggered || _isFading) return;

        // Complete the current objective
        EventBus.Instance.AddObjectiveProgress(lastEvent, progressValue);
        _objectiveTriggered = true;

        // Start coroutine to create the new objective
        StartCoroutine(CreateNewObjective());
    }

    private void Update()
    {
        // Close the note with 'E' key if it's open
        if (_isNoteOpen && Input.GetKeyDown(KeyCode.E) && !_isFading)
        {
            CloseNote();
        }
    }

    private void CloseNote()
    {
        if (notePanelCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(notePanelCanvasGroup, 0f, fadeDuration));
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

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        _isFading = true;

        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        _isFading = false;
    }
}