// using UnityEngine;
//
// public class ElevatorButton : MonoBehaviour, IInteractable
// {
//     [Header("Elevator Reference")]
//     [SerializeField] private HorizontalElevator elevator;
//     [SerializeField] private GameObject activateText; // Assign a UI Text element
//
//     public void Interact()
//     {
//         elevator.ToggleElevator();
//     }
//
//     public GameObject InteractionText() => activateText;
//
//
//     private void OnValidate()
//     {
//         if (elevator == null)
//             elevator = GetComponentInParent<HorizontalElevator>();
//     }
// }

using System.Collections;
using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    [Header("Elevator Reference")]
    [SerializeField] private HorizontalElevator elevator;
    [SerializeField] private GameObject activateText; // Assign a UI Text element

    [Header("Required Item")]
    [SerializeField] private ItemData requiredKey; // Assign in Inspector
    [SerializeField] private GameObject errorText; // Assign a UI Text element
    [SerializeField] private float errorDisplayTime = 2f; // Duration to show error message

    private InventoryViewController _inventory;
    private Coroutine _errorCoroutine;

    private void Start()
    {
        _inventory = FindObjectOfType<InventoryViewController>();
        if (errorText != null) errorText.SetActive(false); // Hide error text initially
    }

    public void Interact()
    {
        // Check for required key
        if (requiredKey != null && (_inventory == null || !_inventory.HasItem(requiredKey)))
        {
            if (_errorCoroutine != null) StopCoroutine(_errorCoroutine);
            _errorCoroutine = StartCoroutine(ShowErrorMessage());
            return;
        }

        elevator.ToggleElevator();
    }

    public GameObject InteractionText() => activateText;

    private void OnValidate()
    {
        if (elevator == null)
            elevator = GetComponentInParent<HorizontalElevator>();
    }

    private IEnumerator ShowErrorMessage()
    {
        if (errorText != null)
        {
            errorText.SetActive(true); // Show the error UI
            yield return new WaitForSeconds(errorDisplayTime);
            errorText.SetActive(false); // Hide after delay
        }
        _errorCoroutine = null;
    }
}