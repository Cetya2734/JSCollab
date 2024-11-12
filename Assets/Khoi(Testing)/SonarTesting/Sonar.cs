using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public Texture Texture;
    public Camera Camera;
    public GameObject redDot;

    // Number of raycasts to shoot
    public int numberOfRays = 8;

    // Maximum distance each ray will travel
    public float raycastDistance = 10f;

    // Optional Layer Mask to filter the rays to hit only specific layers
    public LayerMask raycastLayerMask;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            ShootRays();



        }
    }
    void ShootRays()
    {
        // Calculate the angle between each ray
        float angleStep = 360f / numberOfRays;

        for (int i = 0; i < numberOfRays; i++)
        {
            // Calculate the direction of each ray based on the angle
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            // Cast the ray
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, raycastLayerMask);

            // Draw the ray in the Scene view (for debugging purposes)
            Debug.DrawRay(transform.position, direction * raycastDistance, Color.red, 2f);

            // Check if the ray hits anything
            if (hit.collider != null)
            {
                // For example: print the hit object name
                Debug.Log("Hit: " + hit.collider.name);

                Instantiate(redDot, hit.collider.transform);
            }
        }

    }
}
