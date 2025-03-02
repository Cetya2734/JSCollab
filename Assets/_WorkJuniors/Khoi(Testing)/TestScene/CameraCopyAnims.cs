using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCopyAnims : MonoBehaviour
{
    public GameObject camera;  // The camera to follow
    void Update()
    {
        if (camera != null)
        {
            // Set the local position and rotation of this camera to match the target camera
            transform.localPosition = camera.transform.localPosition;
            transform.localRotation = camera.transform.localRotation;

            // Optionally, if you want to copy the local scale as well:
            transform.localScale = camera.transform.localScale;
        }
    }
}
