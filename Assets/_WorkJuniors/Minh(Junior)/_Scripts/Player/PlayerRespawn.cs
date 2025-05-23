using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public static PlayerRespawn Instance;
    private Vector3 lastSafePosition;
    private CharacterController controller;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        controller = GetComponent<CharacterController>();
        lastSafePosition = transform.position; 
    }

    public void UpdateSafePosition(Vector3 newPosition)
    {
        lastSafePosition = newPosition;
        // Optional: Visual indicator
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private System.Collections.IEnumerator RespawnRoutine()
    {
        // Temporarily disable controller to move the transform
        controller.enabled = false;
        transform.position = lastSafePosition;

        // Tiny delay to ensure clean reactivation
        yield return null;

        controller.enabled = true;
        ScreenFader.Instance.FadeFromBlack(0.5f, null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            ScreenFader.Instance.FadeToBlack(1f, () => {
                Respawn();
            });

        }
    }
}