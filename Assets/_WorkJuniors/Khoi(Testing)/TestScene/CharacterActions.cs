using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class CharacterActions : MonoBehaviour
{
    // Character movement and animation
    [SerializeField] private Animator animator;
    [SerializeField] private Animator camAnimator;
    public bool IsRunning;
    public bool canRun = true;

    // Revolver attributes
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public int maxAmmo = 6;
    public int currentAmmo;
    public float reloadTime = 1f;

    private bool isReloading = false;
    private bool isRecoilPlaying = false;

    [Space(10)]
    // References to components and game objects
    public GameObject fpsCam;                 
    public ParticleSystem muzzleFlash;          
    public GameObject impactEffect;          
    public GameObject bulletCasing;       
    
    [Space(10)]
    // Eject Used Casing
    public Transform casingLocation;
    public TrailRenderer bulletTracer;
    public Transform tracerLocation;
    
    [Space(10)]
    [FormerlySerializedAs("hitSound")] public AudioClip shootSound;             
    public AudioClip noAmmoSound;             
    public AudioClip reloadSound;             
    public AudioClip aimSound;
    public AudioClip breathingSound;

    private float nextTimeToFire = 0f;
    [Space(10)]
    public TextMeshProUGUI ammoText;
    private CrosshairFeedback crosshair;
    [Space(10)]
    public GameObject muzzleFlashes;
    public GameObject muzzleFlashesLight;
    void Start()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        crosshair = FindObjectOfType<CrosshairFeedback>(); // Find the crosshair script
        if (crosshair != null)
        {
            crosshair.SetRateOfFire(fireRate/10); // Pass rate of fire to crosshair
        }

        ToggleAmmoText(true);
        muzzleFlashes.SetActive(false);
    }

    void OnEnable()
    {
        UpdateAmmoText();
        ToggleAmmoText(true);
        isReloading = false;
    }

    private void OnDisable()
    {
        ToggleAmmoText(false);
    }

    void Update()
    {
        // Handle character movement and animations
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("IsWalking", true);
            camAnimator.SetBool("IsWalking", true);
            if (Input.GetKey(KeyCode.LeftShift) && canRun == true)
            {
                animator.SetBool("IsRunning", true);
                AudioManager.Instance.PlayLoopingSound( "breathingSound", breathingSound, this.transform.position, 1f);
                IsRunning = true;
                animator.SetBool("IsWalking", false);
                camAnimator.SetBool("IsRunning", true);
            }
            else
            {
                IsRunning = false;
                AudioManager.Instance.StopLoopingSound("breathingSound");

                camAnimator.SetBool("IsRunning", false);
                animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            camAnimator.SetBool("IsWalking", false);
        }

        // Handle shooting input
        if (!isReloading && Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire && !isRecoilPlaying && !IsRunning)
        {
            if (currentAmmo == 0)
            {
                AudioManager.Instance.PlaySound(noAmmoSound, this.transform.position);
                return;
            }
            nextTimeToFire = Time.time + 1f / fireRate;
            StartCoroutine(Recoil());
            animator.SetTrigger("Shoot");
            camAnimator.SetTrigger("Shoot");
            Shoot();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < 6 && !isReloading && maxAmmo > 0)
        {
            StartCoroutine(Reload());
            animator.SetTrigger("reload");
            camAnimator.SetTrigger("reload");
            animator.SetLayerWeight(3, 1);
        }
        
        if (Input.GetMouseButtonDown(1)) // Right mouse button for aiming
        {
            CameraManager.Instance.ToggleAim(true);
            canRun = false;
            AudioManager.Instance.PlaySound(aimSound, this.transform.position);
        }
        else if (Input.GetMouseButtonUp(1)) // Release to stop aiming
        {
            CameraManager.Instance.ToggleAim(false);
            canRun = true;
        }
    }

    // Logic for shooting
    void Shoot()
    {
        canRun = false;
        // Feedback
        muzzleFlash.Play();
        StartCoroutine(MuzzleFlash());
       AudioManager.Instance.PlaySound(shootSound, this.transform.position);

        if (crosshair != null)
        {
            crosshair.SetRateOfFire(fireRate/10);
            crosshair.AnimateCrosshair();
        }
        
        currentAmmo--;
        UpdateAmmoText();

        // Create casing and impact effects
        GameObject casing = Instantiate(bulletCasing, casingLocation.position, casingLocation.rotation);
        Destroy(casing, 3f);

        // Apply force to the casing
        casing.GetComponent<Rigidbody>().AddForce(transform.forward * -20);

        // Ray cast to detect hit
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            TrailRenderer trail = Instantiate(bulletTracer, tracerLocation.transform.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));
            // Check for hitbox first
            HitBox hitbox = hit.transform.GetComponent<HitBox>();
            if (hitbox != null)
            {
                StartCoroutine(DelayedDamage(hitbox, damage, hit.point));
            }
            else
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    StartCoroutine(DelayedDamage(target, damage, hit.point));

                }
            }

        }
    }
    
    // Coroutine to delay damage
    IEnumerator DelayedDamage(Target target, float damage, Vector3 hitPoint)
    {
        yield return new WaitForSeconds(0.2f); // 50 milliseconds
        target.TakeDamage(damage, hitPoint, false);
    }

    IEnumerator DelayedDamage(HitBox hitbox, float damage, Vector3 hitPoint)
    {
        yield return new WaitForSeconds(0.2f); // 50 milliseconds
        hitbox.TakeDamage(damage, hitPoint);
    }
    
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {

        float time = 0;
        Vector3 startPos = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        
        Destroy(trail.gameObject, trail.time);
    }

    // Update the ammo UI text
    public void UpdateAmmoText()
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    // Toggle the visibility of the ammo UI text
    private void ToggleAmmoText(bool isActive)
    {
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(isActive);
        }
    }

    // Coroutine for recoil animation
    IEnumerator Recoil()
    {
        isRecoilPlaying = true;
        yield return new WaitForSeconds(1f);
        isRecoilPlaying = false;
    }
    
    IEnumerator Reload()
    {
        // Disable switching during reload
        AudioManager.Instance.PlaySound(reloadSound, this.transform.position);
        canRun = false;
        // Set reloading flag
        isReloading = true;
        yield return new WaitForSeconds(reloadTime - 0.25f);

        // Calculate the missing ammo
        int missingAmmo = 6 - currentAmmo; // How many bullets are needed to fill the clip
        int ammoToReload = Mathf.Min(missingAmmo, maxAmmo); // Don't reload more than what's available in maxAmmo

        // Add the missing bullets to currentAmmo
        currentAmmo += ammoToReload;

        // Subtract the reloaded bullets from maxAmmo
        maxAmmo -= ammoToReload;

        // Ensure maxAmmo doesn't go below 0
        if (maxAmmo < 0)
            maxAmmo = 0;

        // Update the ammo UI
        UpdateAmmoText();

        // Reset reloading flag and animation layer weight
        isReloading = false;
        animator.SetLayerWeight(3, 0);
    }
    
    IEnumerator MuzzleFlash()
    {       
        muzzleFlashes.SetActive(true);
        muzzleFlashesLight.SetActive(true);

        yield return new WaitForSeconds(0.03f);
        muzzleFlashes.SetActive(false);

        yield return new WaitForSeconds(0.06f);
        muzzleFlashesLight.SetActive(false);
    }
    // for animation to call
    public void SetRun(int run)
    {
        switch (run)
        {
            case 0:  
                canRun = false; 
                break;
            case 1:
                canRun = true;
                break;
        }
    }
    
    public void AddAmmo(int amount)
    {
        maxAmmo += amount;
        UpdateAmmoText();
        Debug.Log($"Ammo added! Total: {maxAmmo}");
    }
}
