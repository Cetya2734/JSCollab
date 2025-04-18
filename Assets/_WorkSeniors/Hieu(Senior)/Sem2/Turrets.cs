using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    public Transform player;
    public Transform gun;           // The gun barrel that moves
    public Transform bulletSpawn;   // Where bullets spawn
    public GameObject bulletPrefab;
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    public float bulletSpeed = 20f;
    public float detectionRange = 15f;

    [Header("Recoil Settings")]
    public float recoilAmount = 0.2f;  // Move gun backward
    public float recoilRecoverySpeed = 5f;  // Speed to reset position

    private float nextFireTime = 0f;
    private Vector3 originalGunPosition;

    void Start()
    {
        originalGunPosition = gun.localPosition; // Store original position
    }

    void Update()
    {
        if (player == null) return;

        // Rotate turret to face the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Prevent tilting up/down

        if (direction.magnitude <= detectionRange)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Fire bullets at intervals
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }

        // Smoothly recover from recoil
        gun.localPosition = Vector3.Lerp(gun.localPosition, originalGunPosition, Time.deltaTime * recoilRecoverySpeed);
    }

    void Shoot()
    {
        // Instantiate bullet at bulletSpawn position
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawn.forward * bulletSpeed;
        }
        Destroy(bullet, 5f);
        
        gun.localPosition -= gun.forward * recoilAmount;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
