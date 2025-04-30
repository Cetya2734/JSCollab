using UnityEngine;
using System.Collections;

public class PlatformFlap : MonoBehaviour
{
    public Transform doorMesh;
    public AudioSource audioSource;
    public AudioClip openClip;
    public AudioClip closeClip;

    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float flapInterval = 5f;

    public MeshRenderer meshRenderer;
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

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        SetPlatformColor(normalColor);
        StartCoroutine(AutoFlapLoop());
    }

    IEnumerator AutoFlapLoop()
    {
        while (true)
        {
            // Warning
            SetPlatformColor(warningColor);
            yield return new WaitForSeconds(1f);

            StartCoroutine(ShakePlatform(1f, 2f));

            SetPlatformColor(dangerColor);
            yield return new WaitForSeconds(0.5f);

            // Open
            isOpen = true;
            PlaySound(openClip);

            yield return new WaitForSeconds(2f); // Stay open

            // Close
            isOpen = false;
            PlaySound(closeClip);

            SetPlatformColor(normalColor);
            yield return new WaitForSeconds(flapInterval);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        float maxDistance = 25f;

        if (audioSource != null)
        {
            audioSource.enabled = distance <= maxDistance;
        }

        doorMesh.localRotation = Quaternion.Slerp(
            doorMesh.localRotation,
            isOpen ? openRotation : closedRotation,
            Time.deltaTime * openSpeed
        );
    }

    private void SetPlatformColor(Color color)
    {
        if (meshRenderer != null)
            meshRenderer.material.color = color;
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

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
