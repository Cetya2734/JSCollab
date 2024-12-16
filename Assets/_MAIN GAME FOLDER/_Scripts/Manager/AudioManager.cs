using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootSound; // Example shoot sound
    [SerializeField] private AudioClip backgroundMusic; // Example background music

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        // Subscribe to the OnPlayAudio event to handle audio playback
        AudioEventManager.OnPlayAudio += PlayAudio;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        AudioEventManager.OnPlayAudio -= PlayAudio;
    }

    // Play audio based on the triggered event
    private void PlayAudio(string audioName)
    {
        switch (audioName)
        {
            case "shoot":
                audioSource.PlayOneShot(shootSound); // Play shooting sound
                break;

            case "backgroundMusic":
                if (!audioSource.isPlaying || audioSource.clip != backgroundMusic) // Avoid restarting music if it's already playing
                {
                    audioSource.clip = backgroundMusic;
                    audioSource.loop = true;
                    audioSource.Play(); // Start background music
                }
                break;

            default:
                Debug.LogWarning("Audio event not recognized: " + audioName);
                break;
        }
    }

    // Example method to stop the background music
    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }
}