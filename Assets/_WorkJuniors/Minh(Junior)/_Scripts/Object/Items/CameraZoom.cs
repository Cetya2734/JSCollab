using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static System.TimeZoneInfo;

public class CameraZoom : MonoBehaviour
{
    public Camera cameraView;

    [SerializeField] private float _initialFOV;
    [SerializeField] private float _zoomInFOV;
    private float currentFOV;

    public PostProcessVolume postFX;

    // Individual settings within the Post-Processing profile
    ColorGrading colorGrading;
    Grain grain;
    Vignette vignette;
    AutoExposure exposure;

    void Start()
    {
        if (cameraView == null)
        {
            cameraView = GameObject.Find("cameraView")?.GetComponent<Camera>();

            if (cameraView == null)
            {
                Debug.LogError("CameraView not found or not assigned.");
                return;
            }

            cameraView.fieldOfView = _initialFOV;
        }

        if (postFX == null)
        {
            Debug.LogError("PostFX not assigned.");
            return;
        }

        postFX.profile.TryGetSettings(out colorGrading);
        postFX.profile.TryGetSettings(out grain);
        postFX.profile.TryGetSettings(out vignette);
        postFX.profile.TryGetSettings(out exposure);

    }


    void Update()
    {
        // Toggle the active state of various Post-Processing effects on "Fire1" input
        if (Input.GetButtonDown("Fire1"))
        {
            colorGrading.active = !colorGrading.active;
            grain.active = !grain.active;
            vignette.active = !vignette.active;
            exposure.active = !exposure.active;
        }

        // Adjust camera field of view while "Fire2" is held down
        if (Input.GetButton("Fire2"))
        {
            cameraView.fieldOfView = _zoomInFOV;
        }

        else
        {
            // Reset the field of view to the initial value when "Fire2" is released
            cameraView.fieldOfView = _initialFOV;
        }

        currentFOV = Camera.main.fieldOfView;

    }
}
