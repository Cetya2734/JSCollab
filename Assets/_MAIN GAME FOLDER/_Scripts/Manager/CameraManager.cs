using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    
    [Header("Shake Settings")]
    public float defaultShakeDuration = 0.3f;
    public float defaultShakeStrength = 0.5f;
    public int defaultShakeVibration = 10;
    public float defaultShakeRandomness = 90f;

    [Header("Aiming Settings")]
    public float aimFOV = 55f; // Adjust FOV for zoom effect
    public float defaultFOV = 65f;
    public float aimTransitionDuration = 0.2f;


    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }

    public void ShakeCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.DOShakePosition(defaultShakeDuration, defaultShakeStrength, defaultShakeVibration, defaultShakeRandomness);
        }
    }
    
    public void ToggleAim(bool isAiming)
    {
        float targetFOV = isAiming ? aimFOV : defaultFOV;
        mainCamera.DOFieldOfView(targetFOV, aimTransitionDuration);
    }
    
    
    public void ShakeCamera(float duration, float strength, int vibrato = 10, float randomness = 90f)
    {
        if (mainCamera != null)
        {
            mainCamera.transform.DOShakePosition(duration, strength, vibrato, randomness);
        }
    }

    public void ExplosionShake()
    {
        ShakeCamera(0.5f, 1.2f, 20, 100f);
    }

    public void ImpactShake()
    {
        ShakeCamera(0.3f, 0.6f, 15, 50f);
    }
}