using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LadderSystem : MonoBehaviour
{
    [Header("Settings")]
    public float climbSpeed = 4f;
    public string ladderTag = "Ladder";

    [Header("References")]
    public MonoBehaviour playerMovementScript; // Assign your movement script here

    private CharacterController controller;
    private bool isClimbing = false;
    private Transform currentLadder;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        if (playerMovementScript == null)
            Debug.LogError("Assign your movement script in the inspector!");
    }

    void Update()
    {
        if (isClimbing)
        {
            HandleClimbingMovement();
        }
    }

    void HandleClimbingMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 climbDirection = currentLadder.up * verticalInput;
        controller.Move(climbDirection * climbSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ladderTag))
        {
            StartClimbing(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ladderTag))
        {
            StopClimbing();
        }
    }

    void StartClimbing(Transform ladderTransform)
    {
        isClimbing = true;
        currentLadder = ladderTransform;
        
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;
    }

    void StopClimbing()
    {
        isClimbing = false;
        currentLadder = null;
        
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }
}