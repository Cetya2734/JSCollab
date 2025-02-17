using UnityEngine;

interface IInteractable
{
    public void Interact();
    GameObject GetGameObject();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public GameObject ePrompt;
    
    public LayerMask interactableLayer;

    private GameObject currentInteractable;
    void Update()
    {
        Ray secondaryRay = new Ray(InteractorSource.position, InteractorSource.forward);

        Debug.DrawRay(secondaryRay.origin, secondaryRay.direction * InteractRange, Color.green);
        if (Input.GetButtonDown("Interact"))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                    HandleInteractedObjectDestroyed();
                }
            }
        }

        if (Physics.Raycast(secondaryRay, out RaycastHit hitInfoSecondary, InteractRange, interactableLayer))
        {
            if (hitInfoSecondary.collider.gameObject != currentInteractable)
            {
                // Player is looking at a new interactable object
                currentInteractable = hitInfoSecondary.collider.gameObject;
                ePrompt.gameObject.SetActive(true);

                GameObject interactableObject = currentInteractable.GetComponent<IInteractable>().GetGameObject();
                interactableObject.SetActive(true);
            }
        }
        else
        {
            // Player is not looking at any interactable object
            if (currentInteractable != null)
            {
                // Disable action description text
                ePrompt.gameObject.SetActive(false);

                // Access the IInteractable interface to get the GameObject
                GameObject interactableObject = currentInteractable.GetComponent<IInteractable>().GetGameObject();

                // Set the GameObject inactive
                interactableObject.SetActive(false);

                currentInteractable = null;
            }
        }
    }

    void HandleInteractedObjectDestroyed()
    {
        ePrompt.gameObject.SetActive(false);
    }
}
