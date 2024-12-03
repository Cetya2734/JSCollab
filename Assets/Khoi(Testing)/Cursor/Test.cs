using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    // 2D camera
    public Camera camera2D;
    // 3D camera
    public Camera camera3D;
    // meshrenderer de display cai camera 2D
    public MeshRenderer meshRenderer;

    /// <summary>
    /// solution o day kha phuc tap, co ma t gioi
    /// convert screen solution cua cai meshrenderer thanh kich thuoc cua 2D camera
    /// o day anh dung bounds cua mesh renderer, vi` no co cung` 1 ti le (16:9) (1920:1080) (192:108)
    /// vi minh dung de quy doi he tham so tu meshrenderer sang camera, nen anh recommend dung convert sang screen space
    /// check xem chuot co nam trong thang meshrenderer khong
    /// </summary>
    void Update()
    {
        if (camera2D == null || camera3D == null || meshRenderer == null)
            return;

        // lay bounds cua meshrenderer
        Bounds meshBounds = meshRenderer.bounds;

        Vector3 meshMinScreen = camera3D.WorldToScreenPoint(meshBounds.min);
        Vector3 meshMaxScreen = camera3D.WorldToScreenPoint(meshBounds.max);

        // lay position cua chuot nhung ma la screen space
        // Input.mousePosition -> return position cua chuot dua tren screen space (canvas)
        Vector3 mousePosScreen = Input.mousePosition;

        // check xem chuot co nam trong bounds cua meshrenderer khong
        if (mousePosScreen.x >= meshMinScreen.x && mousePosScreen.x <= meshMaxScreen.x &&
            mousePosScreen.y >= meshMinScreen.y && mousePosScreen.y <= meshMaxScreen.y)
        {
            // Map mouse position cho UV coordinates
            // UV coordinate la gi` thi tu tim hieu nhes, dai khai laf 2D tren 3D
            float uvX = (mousePosScreen.x - meshMinScreen.x) / (meshMaxScreen.x - meshMinScreen.x);
            float uvY = (mousePosScreen.y - meshMinScreen.y) / (meshMaxScreen.y - meshMinScreen.y);

            //Debug.Log($"UV Coordinates: ({uvX}, {uvY})");

            // Map UV cho 2D camera world space
            Vector3 screenPos2D = new Vector3(
                uvX * camera2D.pixelWidth,
                uvY * camera2D.pixelHeight,
                0f
            );

            Vector3 worldPos2D = camera2D.ScreenToWorldPoint(screenPos2D);
            // 2D nen cho z = 0 cmnl
            worldPos2D.z = 0;

            //Debug.Log($"World Position: {worldPos2D}");

            // set cursor position
            transform.position = worldPos2D;
        }
        else
        {
            //Debug.Log("Mouse is outside of the MeshRenderer bounds.");
        }
    }
}
