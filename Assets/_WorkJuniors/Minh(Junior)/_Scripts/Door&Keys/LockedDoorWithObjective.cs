using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorWithObjective : MonoBehaviour
{
    // Existing variables
    public Transform doorMesh;
    public AudioSource doorSound;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float closeDelay = 3f;
    public bool isSlidingDoor = false;
    
    [SerializeField] private ItemData requiredKey; // Assign in Inspector
    [SerializeField] private GameObject errorText; // Assign a UI Text element
    [SerializeField] private GameObject openOrCloseText; // Assign a UI Text element
    [SerializeField] private float errorDisplayTime = 2f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private InventoryViewController inventory;
    private Coroutine errorCoroutine;

    void Start()
    {
        inventory = FindObjectOfType<InventoryViewController>();
        Debug.Log("Inventory found: " + (inventory != null));
        
        if (doorMesh == null)
            doorMesh = transform; // Dùng chính đối tượng này nếu quên gán
        
        closedRotation = doorMesh.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
        
        if (errorText != null)
            errorText.SetActive(false); // Hide initially
    }

    public void Interact()
    {
        Debug.Log("Interacted");
        bool hasKey = inventory != null && inventory.HasItem(requiredKey);
        Debug.Log("Has key: " + hasKey);
        
        if (requiredKey != null && !inventory.HasItem(requiredKey))
        {
            // Show error message
            Debug.Log("No Key!");
            if (errorCoroutine != null) StopCoroutine(errorCoroutine);
            errorCoroutine = StartCoroutine(ShowErrorMessage());
            return;
        }

        ToggleDoor();
    }

    private IEnumerator ShowErrorMessage()
    {
        // errorText.gameObject.SetActive(true);
        // yield return new WaitForSeconds(errorDisplayTime);
        // errorText.gameObject.SetActive(false);
        // errorCoroutine = null;
        if (errorText != null)
        {
            errorText.SetActive(true); // Show the error UI
            yield return new WaitForSeconds(errorDisplayTime);
            errorText.SetActive(false); // Hide after delay
        }
        errorCoroutine = null;
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
    
    public GameObject InteractionText()
    {
        return openOrCloseText;
    }
}
