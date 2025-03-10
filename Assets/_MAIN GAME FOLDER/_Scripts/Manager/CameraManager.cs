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

    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }

    /// <summary>
    /// Shakes the camera with default settings.
    /// </summary>
    public void ShakeCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.DOShakePosition(defaultShakeDuration, defaultShakeStrength, defaultShakeVibration, defaultShakeRandomness);
        }
    }

    /// <summary>
    /// Customizable camera shake.
    /// </summary>
    /// <param name="duration">How long the shake lasts.</param>
    /// <param name="strength">How strong the shake is.</param>
    /// <param name="vibrato">Number of vibrations.</param>
    /// <param name="randomness">Random variation in the shake.</param>
    public void ShakeCamera(float duration, float strength, int vibrato = 10, float randomness = 90f)
    {
        if (mainCamera != null)
        {
            mainCamera.transform.DOShakePosition(duration, strength, vibrato, randomness);
        }
    }

    /// <summary>
    /// Predefined strong shake for explosions.
    /// </summary>
    public void ExplosionShake()
    {
        ShakeCamera(0.5f, 1.2f, 20, 100f);
    }

    /// <summary>
    /// Predefined mild shake for light impacts.
    /// </summary>
    public void ImpactShake()
    {
        ShakeCamera(0.2f, 0.4f, 15, 50f);
    }
}