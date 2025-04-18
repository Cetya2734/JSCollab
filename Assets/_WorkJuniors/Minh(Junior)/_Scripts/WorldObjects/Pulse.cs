using UnityEngine;

public class Pulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;         // Speed of pulsing
    public float pulseStrength = 0.2f;    // Intensity of the squash/stretch
    public Vector3 pulseDirection = new Vector3(1f, -0.5f, 1f); // Axis of squash/stretch

    private Vector3 initialScale;
    private float pulseTimer;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        pulseTimer += Time.deltaTime * pulseSpeed;

        // Ping-pong factor between -1 and 1
        float pulse = Mathf.Sin(pulseTimer) * pulseStrength;

        // Calculate squash/stretch scale
        Vector3 stretch = new Vector3(
            1f + pulse * pulseDirection.x,
            1f + pulse * pulseDirection.y,
            1f + pulse * pulseDirection.z
        );

        transform.localScale = Vector3.Scale(initialScale, stretch);
    }
}