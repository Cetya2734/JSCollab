using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurretController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject chargeProjectile;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private int maxAmmo = 100;
    [SerializeField] private float fireRate = 0.5f; 
    
    [SerializeField] private bool isCharging;
    [SerializeField] private float chargeTime;
    [SerializeField] private GameObject chargeVFX;

    [SerializeField] private Animator smallTurret;
    private int currentAmmo;
    private float nextFireTime;

    [Header("Overheat")]
    [SerializeField] private int maxShotsBeforeOverheat = 5; 
    [SerializeField] private float overheatRecoveryTime = 3f;
    private int shotCount = 0;
    private bool isOverheated = false;

    public Test cursorScript;
    public ShootingManager shootingEventManager;
    public AudioEventManager audioEventManager; // Reference to AudioEventManager

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        RotateTurret();

        // Charging logic
        if (Input.GetMouseButton(0) && currentAmmo > 0 && !isOverheated)
        {
            // isCharging = true;
            // chargeTime += Time.deltaTime;

            if (!isCharging) // Ensure isCharging is set only once when starting the charge
            {
                isCharging = true;
                chargeTime = 0f; // Reset charge time at the start of charging
                smallTurret.SetBool("IsCharging", true);
            }

            chargeTime += Time.deltaTime;
            
            if (chargeTime > 0.1f)
            {
                chargeVFX.SetActive(true); // Enable charge effect
            }
        }

        // Fire logic on release
        if (Input.GetMouseButtonUp(0) && Time.time >= nextFireTime && currentAmmo > 0 && !isOverheated)
        {
            if (chargeTime >= 1f) // If the player held long enough for a charged shot
            {
                ChargeShoot(); 
            }
            else if (chargeTime < 1f) // Otherwise, fire a regular shot
            {
                Shoot();
                shootingEventManager.TriggerShootEvent();
                AudioEventManager.TriggerAudioEvent("shoot");
            }

            // Reset charging state
            isCharging = false;
            chargeTime = 0f;
            chargeVFX.SetActive(false); // Disable charge effect
            smallTurret.SetBool("IsCharging", false);

        }
    }

    private void RotateTurret()
    {
        Vector3 mousePosition = cursorScript.worldPos2D;
        mousePosition.z = 0;

        Vector3 direction = mousePosition - transform.position;
//        Debug.Log(transform.position);

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
        //Debug.Log(mousePosition + " | " + targetAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        // Instantiate projectile
        GameObject projectileShot = Instantiate(projectile, spawnLocation.transform.position, transform.rotation);
        Rigidbody2D rb = projectileShot.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * projectileSpeed;
        smallTurret.Play("Knockback", 0, 0f);
        
        // Update ammo, fire rate, and overheat counters
        currentAmmo--;
        shotCount++;
        nextFireTime = Time.time + fireRate;
        
        // Check if turret should overheat
        if (shotCount >= maxShotsBeforeOverheat)
        {
            isOverheated = true;
            StartCoroutine(OverheatRecovery());
        }
        smallTurret.SetBool("IsCharging", false); // Explicitly reset the charge animation on normal fire
    }
    
    void ChargeShoot()
     {
         GameObject projectileShot = Instantiate(chargeProjectile, spawnLocation.transform.position, transform.rotation);
         Rigidbody2D rb = projectileShot.GetComponent<Rigidbody2D>();
         rb.velocity = transform.right * projectileSpeed;

         isCharging = false;
         chargeTime = 0;
         shootingEventManager.TriggerShootEvent();
         smallTurret.SetBool("IsCharging", false); // Stop the charging animation after shooting
     }

    private IEnumerator OverheatRecovery()
    {
        // Wait for the overheat recovery period
        yield return new WaitForSeconds(overheatRecoveryTime);
        // Reset shot count and overheat status
        isOverheated = false;
        shotCount = 0;
    }

    public void ReloadAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }

    public void UpgradeFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    public void UpgradeOverheatThreshold(int newMaxShotsBeforeOverheat)
    {
        maxShotsBeforeOverheat = newMaxShotsBeforeOverheat;
    }

    public void DealDamage(float amount, GameObject target)
    {
        // Health healthComponent = target.GetComponent<Health>();
        // if (healthComponent != null)
        // {
        //     healthComponent.TakeDamage(amount);
        // }
    }
}

