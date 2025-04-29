using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [Header("Timing")]
    [SerializeField] private float delayDuration = 1f; // Delay before creating new objective
    [SerializeField] private float fadeDuration = 0.5f; // Duration for fade in/out

    [Header("UI")]
    [SerializeField] private CanvasGroup notePanelCanvasGroup; // Reference to the CanvasGroup on the note panel

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