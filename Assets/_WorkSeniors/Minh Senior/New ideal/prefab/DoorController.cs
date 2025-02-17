using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public Transform door; // Assign the "DoorPivot" in the Inspector
    public float openAngle = 90f; // How much the door rotates
    public float openSpeed = 2f;  // Speed of rotation
    public float autoCloseTime = 10f; // Time before door auto closes

    private bool isOpen = false;
    private bool isLeftSide = true; // Track which side the player is on
    private Quaternion closedRotation;
    private Quaternion openRotationLeft;
    private Quaternion openRotationRight;
    private Coroutine closeDoorCoroutine;

    void Start()
    {
        closedRotation = door.rotation;
        openRotationLeft = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0)); // Open in one direction
        openRotationRight = Quaternion.Euler(door.eulerAngles + new Vector3(0, -openAngle, 0)); // Open in opposite direction
    }

    public void ToggleDoor(Vector3 playerPosition)
    {
        // Determine which side the player is on
        isLeftSide = Vector3.Dot(transform.forward, (playerPosition - transform.position).normalized) < 0;

        isOpen = true; // Always open first
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isLeftSide ? openRotationLeft : openRotationRight));

        // Start auto-close countdown
        if (closeDoorCoroutine != null)
        {
            StopCoroutine(closeDoorCoroutine);
        }
        closeDoorCoroutine = StartCoroutine(AutoCloseDoor());
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(door.rotation, targetRotation) > 0.1f)
        {
            door.rotation = Quaternion.Slerp(door.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
        door.rotation = targetRotation; // Ensure final position is exact
    }

    private IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(autoCloseTime);
        isOpen = false;
        StartCoroutine(RotateDoor(closedRotation));
    }
}

