// using System.Collections;
// using UnityEngine;
//
// public class AudioManager : MonoBehaviour
// {
//     
//     [SerializeField] private AudioSource _musicSource;
//     [SerializeField] private AudioSource _soundsSource;
//     
//     [Header("Audio Clips")]
//     [SerializeField] private AudioClip shootSound; // Example shoot sound
//     [SerializeField] private AudioClip backgroundMusic; // Example background music
//
//     [SerializeField] AudioSource audioSource;
//     
//     public static AudioManager Instance { get; private set; }
//
//     private void Awake()
//     {
//         // Subscribe to the OnPlayAudio event to handle audio playback
//         
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject); // Keep AudioManager between scenes
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//
//         AudioEventManager.OnPlayAudio += PlayAudio;
//     }
//
//     private void OnDestroy()
//     {
//         // Unsubscribe from the event to avoid memory leaks
//         AudioEventManager.OnPlayAudio -= PlayAudio;
//     }
//
//     private void Start()
//     {
//         PlayMusic(_musicSource.clip);
//     }
//
//     // Play audio based on the triggered event
//     private void PlayAudio(string audioName)
//     {
//         switch (audioName)
//         {
//             case "shoot":
//                 audioSource.PlayOneShot(shootSound); // Play shooting sound
//                 break;
//
//             case "backgroundMusic":
//                 if (!audioSource.isPlaying || audioSource.clip != backgroundMusic) // Avoid restarting music if it's already playing
//                 {
//                     audioSource.clip = backgroundMusic;
//                     audioSource.loop = true;
//                     audioSource.Play(); // Start background music
//                 }
//                 break;
//
//             default:
//                 Debug.LogWarning("Audio event not recognized: " + audioName);
//                 break;
//         }
//     }
//
//     // Example method to stop the background music
//     public void StopBackgroundMusic()
//     {
//         audioSource.Stop();
//     }
//     
//     public void PlayMusic(AudioClip clip) {
//         _musicSource.clip = clip;
//         _musicSource.Play();
//     }
//
//     public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1) {
//         _soundsSource.transform.position = pos;
//         PlaySound(clip, vol);
//     }
//
//     public void PlaySound(AudioClip clip, float vol = 1) {
//         _soundsSource.pitch = Random.Range(0.9f, 1.1f); // Randomize pitch
//         _soundsSource.PlayOneShot(clip, vol);
//         StartCoroutine(ResetPitch()); // Reset pitch after playing
//     }
//     
//     private IEnumerator ResetPitch() {
//         yield return new WaitForSeconds(_soundsSource.clip.length); 
//         _soundsSource.pitch = 1.0f; // Reset pitch to default
//     }
// }

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
