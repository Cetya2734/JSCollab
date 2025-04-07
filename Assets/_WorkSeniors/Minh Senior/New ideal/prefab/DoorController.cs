using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour, IInteractable
{
    public Transform doorMesh; // GameObject chứa Mesh của cửa
    public AudioSource doorSound; // Âm thanh cửa
    public float openAngle = 90f; // Góc mở cửa
    public float openSpeed = 2f; // Tốc độ mở cửa
    public float closeDelay = 3f; // Thời gian chờ để đóng cửa
    public bool isSlidingDoor = false; // Nếu true, cửa sẽ trượt

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    
    [SerializeField] private GameObject interactionText; 

    void Start()
    {
        if (doorMesh == null)
            doorMesh = transform; // Dùng chính đối tượng này nếu quên gán

        closedRotation = doorMesh.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        // Phát âm thanh nếu có gán AudioSource
        if (doorSound != null)
        {
            doorSound.Play();
        }

        // Nếu cửa mở, đợi rồi tự động đóng
        if (isOpen)
        {
            StartCoroutine(CloseDoorAfterDelay());
        }
    }

    IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);
        isOpen = false;

        // Phát âm thanh khi đóng cửa
        if (doorSound != null)
        {
            doorSound.Play();
        }
    }

    void Update()
    {
        doorMesh.localRotation = Quaternion.Slerp(doorMesh.localRotation, isOpen ? openRotation : closedRotation, Time.deltaTime * openSpeed);
    }

    public void Interact()
    {
        ToggleDoor();
    }

    public GameObject InteractionText()
    {
        return interactionText;
    }
}
