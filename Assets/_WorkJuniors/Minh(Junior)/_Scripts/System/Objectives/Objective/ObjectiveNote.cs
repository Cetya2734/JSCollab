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
        Debug.Log("Created");
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