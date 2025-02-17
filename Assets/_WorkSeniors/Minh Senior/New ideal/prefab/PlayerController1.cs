using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed
    public float jumpForce = 7f;  // Jump power
    public float interactRange = 2f; // Interaction range
    public LayerMask interactableLayer; // Layer for interactable objects

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
        Interact();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    Debug.Log("Interacting with " + hit.collider.name);

                    DoorController door = hit.collider.GetComponent<DoorController>();
                    if (door != null)
                    {
                        door.ToggleDoor(transform.position); // Pass player position to decide opening direction
                    }
                }
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
