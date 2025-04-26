using UnityEngine;
using UnityEngine.EventSystems;

public interface IInteractable
{
    public void Interact();
    GameObject InteractionText();
}

public class InteractorWithOutline : MonoBehaviour
{
    public Transform interactorSource; // Source of the interaction ray (e.g., player's camera)
    public float interactRange = 5f; // Range of interaction
    public GameObject ePrompt; // UI prompt for interaction (e.g., "Press E to interact")
    public LayerMask interactableLayer; // Layer for interactable objects

    public AudioClip interactionSound; // Sound to play on interaction
    private GameObject currentInteractable; // Currently focused interactable object
    private Transform highlight; // Currently highlighted object
    private Transform selection; // Currently selected object

    void Update()
    {
        HandleOutline();
        HandleInteraction();
    }
    private void HandleOutline()
    {
        // Keep track of the previous highlight to prevent flickering
        Transform previousHighlight = highlight;

        highlight = null; // Reset highlight reference

        Ray ray = new Ray(interactorSource.position, interactorSource.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.green);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out RaycastHit hitInfo, interactRange, interactableLayer))
        {
            highlight = hitInfo.transform;

            if (highlight.TryGetComponent<IInteractable>(out _))
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    outline.OutlineColor = Color.white;
                    outline.OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null; // Ignore non-interactables
            }
        }

        // Only disable the outline if it's a different object than before
        if (previousHighlight != null && previousHighlight != highlight)
        {
            previousHighlight.gameObject.GetComponent<Outline>().enabled = false;
        }
    }
   
    private void HandleInteraction()
    {
        Ray ray = new Ray(interactorSource.position, interactorSource.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange, interactableLayer))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                // Display the interaction prompt and secondary text
                ePrompt.SetActive(true);

                // Fetch the secondary description text GameObject and enable it
                GameObject interactionText = interactObj.InteractionText();
                if (interactionText != null)
                {
                    interactionText.SetActive(true);
                }

                if (Input.GetButtonDown("Interact"))
                {
                    interactObj.Interact();
                    AudioManager.Instance.PlaySound(interactionSound, hitInfo.point);
                }

                currentInteractable = hitInfo.collider.gameObject;
                return; // Ensure we don't disable the prompt unintentionally
            }
        }

        // Hide the prompt and text when no interactable object is detected
        if (currentInteractable != null)
        {
            ePrompt.SetActive(false);

            if (currentInteractable.TryGetComponent(out IInteractable previousInteractable))
            {
                GameObject interactionText = previousInteractable.InteractionText();
                if (interactionText != null)
                {
                    interactionText.SetActive(false);
                }
            }
        }
    }
}