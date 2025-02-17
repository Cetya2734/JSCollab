using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3f; // Speed of climbing
    public float exitForwardDistance = 1f; // Distance to move forward when exiting the ladder
    public Transform ladderTop; // Reference to the top of the ladder
    public Transform ladderBottom; // Reference to the bottom of the ladder
    private bool isClimbing = false; // State for climbing
    private Transform playerTransform; // Reference to the player transform
    private bool canClimb = false; // State for whether the player can start climbing

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform; // Store the player's transform
            if (this.gameObject.name == "LadderStartPoint")
            {
                canClimb = true; // Allow climbing when entering the start point
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.name == "LadderStartPoint")
            {
                canClimb = false; // Disallow climbing when exiting the start point
            }
            else if (this.gameObject.name == "LadderStopPoint")
            {
                isClimbing = false; // Exit climbing state at the stop point
                // Set the player's position to the top of the ladder if they exit at the top
                if (playerTransform.position.y >= ladderTop.position.y)
                {
                    playerTransform.position = ladderTop.position; // Snap to the top of the ladder
                }
                MovePlayerForward(other.transform); // Move the player forward slightly
            }
        }
    }

    void Update()
    {
        if (isClimbing && canClimb)
        {
            // Climb up
            if (Input.GetKey(KeyCode.E))
            {
                // Check if the player is below the top of the ladder
                if (playerTransform.position.y < ladderTop.position.y)
                {
                    playerTransform.position += new Vector3(0, climbSpeed * Time.deltaTime, 0); // Move player up
                }
            }
            // Climb down
            else if (Input.GetKey(KeyCode.Q))
            {
                // Check if the player is above the bottom of the ladder
                if (playerTransform.position.y > ladderBottom.position.y)
                {
                    playerTransform.position += new Vector3(0, -climbSpeed * Time.deltaTime, 0); // Move player down
                }
            }
        }
    }

    private void MovePlayerForward(Transform player)
    {
        // Move the player forward in the direction they are facing
        Vector3 forwardMove = player.forward * exitForwardDistance;
        player.position += forwardMove; // Move player forward slightly
    }
}
