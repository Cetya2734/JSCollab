using UnityEngine;

public class ControllerSwitcher : MonoBehaviour
{
    public GameObject character;
    public GameObject[] turrets;  // Array of turrets
    public GameObject[] ships;    // Array of ships

    private IControllable activeController;
    private int currentTurretIndex = -1;  // -1 means no turret selected
    private int currentShipIndex = -1;    // -1 means no ship selected

    void Start()
    {
        // Start with the character as the active controller
        SetActiveController(character.GetComponent<IControllable>());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))  // Switch control on pressing "E"
        {
            SwitchControl();
            Debug.Log("pressE");
        }
    }

    void SwitchControl()
    {
        if (activeController == character.GetComponent<IControllable>())
        {
            // Switch from character to a turret if available
            if (turrets.Length > 0)
            {
                currentTurretIndex = (currentTurretIndex + 1) % turrets.Length;
                SetActiveController(turrets[currentTurretIndex].GetComponent<IControllable>());
            }
        }
        else if (activeController is TurretController2D)
        {
            // Switch from turret to ship if available
            if (ships.Length > 0)
            {
                currentShipIndex = (currentShipIndex + 1) % ships.Length;
                SetActiveController(ships[currentShipIndex].GetComponent<IControllable>());
            }
            else
            {
                // Switch back to character if no ships available
                SetActiveController(character.GetComponent<IControllable>());
            }
        }
        else if (activeController is ShipController2D)
        {
            // Switch back to character control from ship
            SetActiveController(character.GetComponent<IControllable>());
        }
    }

    void SetActiveController(IControllable newController)
    {
        if (activeController != null)
        {
            activeController.enabled = false;
        }
        activeController = newController;
        activeController.enabled = true;
    }
}

// Interface to unify control for all types
public interface IControllable
{
    bool enabled { get; set; }
}

// Example Character Controller
public class CharacterController2D : MonoBehaviour, IControllable
{
    // Implement character control logic here
}

// Example Turret Controller
public class TurretController2D : MonoBehaviour, IControllable
{
    // Implement turret control logic here
}

// Example Ship Controller
public class ShipController2D : MonoBehaviour, IControllable
{
    // Implement ship control logic here
}
