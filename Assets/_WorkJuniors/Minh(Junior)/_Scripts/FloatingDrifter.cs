using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingDrifter : MonoBehaviour
{
    [Header("Drift Settings")]
    public float driftStrength = 0.3f;
    public float driftSpeed = 0.5f;

    [Header("Physics Settings")]
    public float waterDrag = 1.2f;
    public float angularDrag = 0.8f;
    public float mass = 1f;

    private Rigidbody rb;
    private Vector3 driftOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.drag = waterDrag;
        rb.angularDrag = angularDrag;
        rb.mass = mass;

        driftOffset = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);
    }

    void FixedUpdate()
    {
        Vector3 drift = new Vector3(
            Mathf.PerlinNoise(Time.time * driftSpeed, driftOffset.x) - 0.5f,
            Mathf.PerlinNoise(Time.time * driftSpeed, driftOffset.y) - 0.5f,
            Mathf.PerlinNoise(Time.time * driftSpeed, driftOffset.z) - 0.5f
        );

        rb.AddForce(drift * driftStrength, ForceMode.Force);
    }
}