using UnityEngine;

public class EyeballLook : MonoBehaviour
{
    public Transform player; // Assign the player's transform (e.g. camera or head)
    public Vector3 offset = Vector3.zero; // Optional offset to fine-tune eye direction
    public float smoothSpeed = 5f; // Higher = snappier, lower = slower

    void Update()
    {
        if (player == null) return;

        // Calculate target rotation
        Vector3 direction = (player.position + offset - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate toward the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}