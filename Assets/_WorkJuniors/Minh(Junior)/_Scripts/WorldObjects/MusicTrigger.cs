// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class MusicTrigger : MonoBehaviour
// {
//     public AudioClip musicClip;
//     public AudioSource source;
//     private void OnCollisionEnter(Collision other)
//     {
//         if(other.tag == "Player")
//         {
//             source.Play();
//         }
//     }
// }


using UnityEngine;

[RequireComponent(typeof(Collider))] // Ensures a collider is attached
public class MusicTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _audioClip; // Sound to play
    [SerializeField] private AudioSource _audioSource; // Optional: Assign an existing AudioSource
    [SerializeField] private bool _playOnce = true; // Play only once?
    [SerializeField] private bool _stopOnExit = false; // Stop sound when player exits?
    [SerializeField] private float _volume = 1f; // Volume control

    private bool _hasPlayed = false;

    private void Awake()
    {
        // If no AudioSource is assigned, create one
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_playOnce && _hasPlayed) return; // Skip if already played

            if (_audioClip != null)
            {
                _audioSource.PlayOneShot(_audioClip, _volume);
                _hasPlayed = true;
            }
            else
            {
                Debug.LogWarning("No audio clip assigned in MusicTrigger!", this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_stopOnExit && other.CompareTag("Player"))
        {
            _audioSource.Stop();
        }
    }
}