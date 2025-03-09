using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FPSController : MonoBehaviour
{
    public bool canMove;

    public static FPSController Instance { get; private set; }

    [SerializeField] private bool isWalking;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float descentMultiplier = 2;
    
    [SerializeField] private MouseLook mouseLook;

    private Camera playerCamera;
    public GameObject cameraArmature;


    private Vector2 input;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private bool previouslyGrounded;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        canMove = true;

        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        mouseLook.Init(transform, playerCamera.transform);
    }

    // Subscribe to and unsubscribe from inventory events
    private void OnEnable()
    {
        EventBus.Instance.onGameplayPaused += () => canMove = false;
        EventBus.Instance.onGameplayResumed += () => canMove = true;
    }

    private void OnDisable()
    {
        EventBus.Instance.onGameplayPaused -= () => canMove = false;
        EventBus.Instance.onGameplayResumed -= () => canMove = true;
    }

    private void Update()
    {
        RotateView();
    }
    private void FixedUpdate()
    {
        // Check if movement is allowed
        if (canMove == false) return;

        float speed;
        GetInput(out speed);

        // Calculate desired movement direction
        Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

        //Check if grounded
        bool isGrounded = IsGrounded();

        // If grounded, reset the Y position
        if (isGrounded)
        {
            moveDirection.y = -characterController.stepOffset; // Adjust as needed
        }
        else
        {
            moveDirection.y -= gravity * Time.fixedDeltaTime * descentMultiplier;
        }

        desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;

        // Update movement direction based on input and ground conditions
        moveDirection.x = desiredMove.x * speed;
        moveDirection.z = desiredMove.z * speed;

        // Move the character controller
        characterController.Move(moveDirection * Time.fixedDeltaTime);

        // Update mouse look and cursor lock
        mouseLook.UpdateCursorLock();
        
    }

    private bool IsGrounded()
    {
         RaycastHit hitInfo;
                return Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
                    characterController.height / 2f + 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
    }
    
    // Get player input and calculate speed
    private void GetInput(out float speed)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool wasWalking = isWalking;

        isWalking = !Input.GetKey(KeyCode.LeftShift);

        speed = isWalking ? walkSpeed : runSpeed;
        input = new Vector2(horizontal, vertical);

        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        if (isWalking != wasWalking && characterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
        }
    }

    // Rotate the player's view using mouse input
    private void RotateView()
    {
        if (canMove == false) return;
        mouseLook.LookRotation(transform, cameraArmature.transform);
    }
}
