using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float rotationSpeed = 5f; // Speed of rotation
    public float minAngle = -45f;     // Minimum rotation angle
    public float maxAngle = 45f;      // Maximum rotation angle

    public GameObject projectilePrefab; // Assign in the inspector
    public float projectileSpeed = 10f;

    void Update()
    {
        RotateTurret();
        if (Input.GetButtonDown("Fire1")) // Shooting control
        {
            Shoot();
        }
    }

    void RotateTurret()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set Z to 0 for 2D

        Vector3 direction = mousePosition - transform.position; // Base's position

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Clamp the angle if needed
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Set the projectile's velocity to shoot downward
        rb.velocity = transform.right * projectileSpeed; // Invert the direction
    }
}