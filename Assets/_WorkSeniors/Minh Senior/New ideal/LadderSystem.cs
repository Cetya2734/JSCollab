using UnityEngine;

public class LadderSystem : MonoBehaviour
{
    public float climbSpeed = 3f;  // Speed of climbing
    private bool isClimbing = false;
    private Rigidbody playerRb;
    private Transform playerTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched ladder.");

            playerTransform = other.transform;
            playerRb = playerTransform.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                playerRb.useGravity = false; // Disable gravity while climbing
                playerRb.velocity = Vector3.zero; // Stop any existing movement
            }

            isClimbing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left ladder.");
            StopClimbing();
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            float verticalInput = 0f;

            if (Input.GetKey(KeyCode.W))
            {
                verticalInput = 1f;  // Move UP
            }
            else if (Input.GetKey(KeyCode.S))
            {
                verticalInput = -1f; // Move DOWN
            }

            // Apply Rigidbody movement
            playerRb.velocity = new Vector3(0, verticalInput * climbSpeed, 0);
        }
    }

    private void StopClimbing()
    {
        isClimbing = false;

        if (playerRb != null)
        {
            playerRb.useGravity = true; // Re-enable gravity
            playerRb.velocity = Vector3.zero; // Stop movement when leaving the ladder
        }

        Debug.Log("Stopped climbing.");
    }
}
