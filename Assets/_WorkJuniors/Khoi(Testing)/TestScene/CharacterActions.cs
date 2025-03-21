// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class CharacterActions : MonoBehaviour
// {
//     [SerializeField] public Animator animator; 
//     [SerializeField] public FPSController FPSController;
//
//     // Update is called once per frame
//     void Update()
//     {
//         if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
//         {
//             animator.SetTrigger("walking");
//             if(Input.GetKey(KeyCode.LeftShift)){
//                 animator.SetTrigger("running");
//             }
//         }
//         if (Input.GetMouseButtonDown(0))
//         {
//             animator.SetTrigger("Shoot");
//
//         }
//         if (Input.GetKey(KeyCode.R))
//         {
//             animator.SetTrigger("reload");
//             animator.SetLayerWeight(3, 1);
//
//         }
//     }
//     
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterActions : MonoBehaviour
{
    // Character movement and animation
    [SerializeField] private Animator animator;
    [SerializeField] private FPSController fpsController;

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

    // References to components and game objects
    public GameObject fpsCam;                   // The Point Of Shooting
    public ParticleSystem muzzleFlash;          // Particle Effect For Muzzle Flash
    public GameObject impactEffect;             // Bullet Impact Effect
    public GameObject bulletCasing;             // Eject Used Casing
    public Transform casingLocation;            // Where The Casing Gets Ejected

    public AudioSource weaponSound;             // Weapon Sound Effect
    public AudioSource noAmmoSound;             // Empty Gun Sound 
    public AudioSource reloadSound;             // Reload Sound 

    private float nextTimeToFire = 0f;

    public TextMeshProUGUI ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        ToggleAmmoText(true);
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
            animator.SetTrigger("walking");
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetTrigger("running");
            }
        }

        // Handle shooting input
        if (!isReloading && Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire && !isRecoilPlaying)
        {
            if (currentAmmo == 0)
            {
                noAmmoSound.Play();
                return;
            }
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            StartCoroutine(Recoil());
            animator.SetTrigger("Shoot");
        }

        // Handle reload input
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo == 0 && maxAmmo >= 6 && !isReloading)
        {
            StartCoroutine(Reload());
            animator.SetTrigger("reload");
            animator.SetLayerWeight(3, 1);
        }
    }

    // Logic for shooting
    void Shoot()
    {
        // // Check for available ammo
        // if (currentAmmo <= 0)
        // {
        //     noAmmoSound.Play();
        //     return;
        // }

        // Play visual and audio effects
        muzzleFlash.Play();
        weaponSound.Play();

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
            // Deal damage to the target
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Create impact effect
            GameObject impactOB = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactOB, 2f);
        }
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

    // Coroutine for reloading
    IEnumerator Reload()
    {
        // Disable switching during reload
        reloadSound.Play();

        // Set reloading flag
        isReloading = true;
        yield return new WaitForSeconds(reloadTime - 0.25f);

        // Reset ammo
        maxAmmo -= 6;
        currentAmmo = 6;

        if (maxAmmo < 0) // Ensure maxAmmo doesn't go below 0
            maxAmmo = 0;

        UpdateAmmoText();

        // Reset reloading flag
        isReloading = false;
    }
}