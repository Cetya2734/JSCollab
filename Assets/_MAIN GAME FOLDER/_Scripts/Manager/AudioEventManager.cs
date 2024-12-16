using System;
using UnityEngine;

public class AudioEventManager : MonoBehaviour
{
    // Define audio events that can be triggered
    public static event Action<string> OnPlayAudio;  // A simple audio event for playing a sound with a sound name/ID

    // Method to trigger the audio event
    public static void TriggerAudioEvent(string audioName)
    {
        OnPlayAudio?.Invoke(audioName);  // Trigger the event with the audio name
    }
}