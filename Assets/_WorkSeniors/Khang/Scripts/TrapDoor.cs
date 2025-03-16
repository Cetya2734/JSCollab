using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public GameObject door; // Assign the door object in the inspector.
    public float closeSpeed = 2.0f; // Speed of door closing.
    public float closeAngle = 90.0f; // Amount the door should rotate to close.
    public bool isDoorOpen = true; // start the door open
    public bool isClosing = false;
    private Quaternion openRotation;
    private Quaternion closedRotation;

    void Start()
    {
        openRotation = door.transform.rotation;
        closedRotation = Quaternion.Euler(door.transform.eulerAngles + new Vector3(0, closeAngle, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isDoorOpen && !isClosing) // Check if the player entered.
        {
            isDoorOpen = false;
            isClosing = true;
            Debug.Log("Player entered, closing door!");
            // Optionally, disable the trigger to prevent multiple activations:
            GetComponent<Collider>().enabled = false;
        }
    }

    void Update()
    {
        if (isClosing)
        {
            door.transform.rotation = Quaternion.Slerp(door.transform.rotation, closedRotation, Time.deltaTime * closeSpeed);

            if (Quaternion.Angle(door.transform.rotation, closedRotation) < 1)
            {
                isClosing = false;
            }
        }
    }
}
