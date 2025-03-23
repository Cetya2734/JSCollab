using UnityEngine;

public class AudioDebugger : MonoBehaviour
{
    private void Awake()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        Debug.Log($"Found {allAudioSources.Length} AudioSources in the scene.");

        foreach (AudioSource source in allAudioSources)
        {
            if (source.isPlaying)
            {
                Debug.Log($"Playing Audio: {source.clip.name} on {source.gameObject.name} with volume: {source.volume}");
            }
        }
    }
}