using UnityEngine;
using System.Collections;

public class PlatformFlap : MonoBehaviour
{
    public Transform doorMesh;
    public AudioSource doorSound;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float flapInterval = 5f;

    public MeshRenderer meshRenderer; // To change color as warning
    public Color normalColor = Color.white;
    public Color warningColor = Color.yellow;
    public Color dangerColor = Color.red;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;

    void Start()
    {
        if (doorMesh == null)
            doorMesh = transform;

        closedRotation = doorMesh.localRotation;
        openRotation = Quaternion.Euler(-openAngle, 0, 0) * closedRotation;

        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        SetPlatformColor(normalColor);
        StartCoroutine(AutoFlapLoop());
    }

    IEnumerator AutoFlapLoop()
    {
        while (true)
        {
            // Warning phase
            SetPlatformColor(warningColor);
            yield return new WaitForSeconds(1f);

            StartCoroutine(ShakePlatform(1f, 2f));

            SetPlatformColor(dangerColor);
            yield return new WaitForSeconds(0.5f);

            // Collapse
            isOpen = true;
            if (doorSound != null) doorSound.Play();

            yield return new WaitForSeconds(2f); // Time it stays open

            // Close
            isOpen = false;
            if (doorSound != null) doorSound.Play();

            SetPlatformColor(normalColor);
            yield return new WaitForSeconds(flapInterval);
        }
    }

    void Update()
    {
        doorMesh.localRotation = Quaternion.Slerp(
            doorMesh.localRotation,
            isOpen ? openRotation : closedRotation,
            Time.deltaTime * openSpeed
        );
    }

    private void SetPlatformColor(Color color)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = color;
        }
    }
    
    IEnumerator ShakePlatform(float duration, float strength)
    {
        float time = 0f;
        while (time < duration)
        {
            float shakeAmount = Mathf.Sin(Time.time * 40f) * strength;
            doorMesh.localRotation = closedRotation * Quaternion.Euler(shakeAmount, 0f, 0f);
            time += Time.deltaTime;
            yield return null;
        }
        doorMesh.localRotation = closedRotation;
    }

}
