using UnityEngine;
using System.Collections;

public class SlidingDoorController : MonoBehaviour
{
    public Transform door; // Assign the door object
    public float slideDistance = 3f; // How far the door moves
    public float slideSpeed = 2f; // Speed of sliding
    public float closeDelay = 3f; // Delay before closing

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private Coroutine closeDoorCoroutine;

    void Start()
    {
        closedPosition = door.position;
        openPosition = closedPosition + new Vector3(0, 0, slideDistance); // Moves along X-axis
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (closeDoorCoroutine != null)
            {
                StopCoroutine(closeDoorCoroutine);
            }
            closeDoorCoroutine = StartCoroutine(CloseDoorAfterDelay());
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            StopAllCoroutines();
            StartCoroutine(MoveDoor(openPosition));
        }
    }

    private IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);
        isOpen = false;
        StartCoroutine(MoveDoor(closedPosition));
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.position, targetPosition) > 0.01f)
        {
            door.position = Vector3.Lerp(door.position, targetPosition, Time.deltaTime * slideSpeed);
            yield return null;
        }
        door.position = targetPosition; // Snap to exact position
    }
}
