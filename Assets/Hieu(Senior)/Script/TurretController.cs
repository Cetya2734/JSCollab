using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f; // Speed of rotation
    [SerializeField] private float _minAngle = -45f;    
    [SerializeField] private float _maxAngle = 45f;      

    [SerializeField] private GameObject _projectilePrefab; 

    [SerializeField] private GameObject _spawnLocation;
    public float projectileSpeed = 10f;

    void Update()
    {
        RotateTurret();
        if (Input.GetButtonDown("Fire1")) 
        {
            Shoot();
        }
    }

    void RotateTurret()
    {
        // Cursor location
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        Vector3 direction = mousePosition - transform.position;
        
        // Clamp the angle 
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, _minAngle, _maxAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _spawnLocation.transform.position, transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Set the projectile's velocity to shoot downward
        rb.velocity = transform.right * projectileSpeed; 
    }
}