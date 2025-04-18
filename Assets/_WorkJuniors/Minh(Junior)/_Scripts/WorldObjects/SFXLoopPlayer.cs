using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXLoopPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    [Range(0f, 1f)] public float spatialBlend = 1f; // 1 = full 3D
    public float volume = 1f;
    public float minDistance = 1f;
    public float maxDistance = 20f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = volume;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        audioSource.Play();
    }
}