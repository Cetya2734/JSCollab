using System.Collections;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using DG.Tweening; // Ensure DOTween is being used

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera sceneCamera;
    private Vector3 originalPosition;  // Store the original position of the camera

    
    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 1f;
    [SerializeField] private int shakeVibration = 10;
    [SerializeField] private float shakeRandomness = 90f;

    private void Awake()
    {
        originalPosition = sceneCamera.transform.localPosition;  // Store the original position
        ShootingManager.OnShoot += ShakeCamera;
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        ShootingManager.OnShoot -= ShakeCamera;
    }

    [ProButton]
    // Method to trigger the camera shake
    public void ShakeCamera()
    {
        // Ensure the camera's position is reset after shaking
        sceneCamera.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibration, shakeRandomness)
            .OnKill(() => sceneCamera.transform.localPosition = originalPosition); // Reset position after shake
    }

    // Method to adjust shake properties dynamically
    public void SetShakeProperties(float duration, float strength, int vibration, float randomness)
    {
        shakeDuration = duration;
        shakeStrength = strength;
        shakeVibration = vibration;
        shakeRandomness = randomness;
    }
}
