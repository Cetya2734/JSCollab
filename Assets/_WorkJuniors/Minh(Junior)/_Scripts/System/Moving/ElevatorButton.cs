using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractable
{
    [Header("Elevator Reference")]
    [SerializeField] private HorizontalElevator elevator;
    [SerializeField] private GameObject activateText; // Assign a UI Text element

    public void Interact()
    {
        elevator.ToggleElevator();
    }

    public GameObject InteractionText() => activateText;


    private void OnValidate()
    {
        if (elevator == null)
            elevator = GetComponentInParent<HorizontalElevator>();
    }
}