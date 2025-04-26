using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;
    
    private Dictionary<string, AudioSource> loopingSounds = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Plays a one-shot sound at a specific position.
    /// </summary>
    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        
        float randomPitch = Random.Range(minPitch, maxPitch);
        GameObject soundObject = new GameObject("OneShotAudio");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = randomPitch;
        audioSource.spatialBlend = 0.3f; // Makes the sound 3D
        audioSource.Play();

        Destroy(soundObject, clip.length); // Clean up after playing
    }

    /// <summary>
    /// Starts playing a looping sound. Uses a key to track it.
    /// </summary>
    public void PlayLoopingSound(string key, AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null || loopingSounds.ContainsKey(key)) return;

        GameObject soundObject = new GameObject($"LoopingAudio_{key}");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        // audioSource.spatialBlend = 1f;
        audioSource.loop = true;
        audioSource.Play();

        loopingSounds[key] = audioSource;
    }

    /// <summary>
    /// Stops a looping sound based on its key.
    /// </summary>
    public void StopLoopingSound(string key)
    {
        if (loopingSounds.TryGetValue(key, out AudioSource source))
        {
            source.Stop();
            Destroy(source.gameObject);
            loopingSounds.Remove(key);
        }
    }
}
