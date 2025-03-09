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
                Debug.Log("Hit Object: " + hit.collider.name); // Kiểm tra Raycast có trúng cửa không?

                DoorController door = hit.collider.GetComponent<DoorController>();
                if (door != null)
                {
                    Debug.Log("Cửa phát hiện, đang mở..."); // Kiểm tra có nhận diện đúng không?
                    door.ToggleDoor();
                }
                else
                {
                    Debug.Log("Không tìm thấy DoorController!"); // Lỗi: Không có Script trên cửa
                }
            }
            else
            {
                Debug.Log("Không chạm vào cửa!"); // Lỗi: Không trúng gì cả
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

