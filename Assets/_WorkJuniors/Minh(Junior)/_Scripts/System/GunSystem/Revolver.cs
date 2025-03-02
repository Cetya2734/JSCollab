    using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;


public class Revolver : MonoBehaviour
{
    // Public variables for customizable revolver attributes
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    [Space(10)]
    public int maxAmmo = 6;
    public int currentAmmo;
    public float reloadTime = 1f;
    [Space(10)]

    private bool isReloading = false;
    private bool isRecoilPlaying = false;
    [Space(10)]

    // References to components and game objects
    public Animator anim;
    [Space(10)]

    public GameObject fpsCam;                   //The Point Of Shooting
    public ParticleSystem muzzleFlash;          //Particle Effect For Muzzle Flash
    public GameObject impactEffect;             //Bullet Impact Effect
    public GameObject bulletCasing;             //Eject Used Casing
    public Transform casinglocation;            //Where The Casing Gets Ejected
    [Space(10)]
    public AudioSource weaponSound;             //Weapon Sound Effect
    public AudioSource noAmmoSound;             //Empty Gun Sound 
    public AudioSource reloadSound;             //Reload Sound 
    [Space(10)]

    private float nextTimeToFire = 0f;
    [Space(10)]
    [Space(10)]

    public GameObject gun;
    [Space(10)]
    public TextMeshProUGUI ammoText;                

    void Start()
    {
        isReloading = false;
        ToggleAmmoText(false);
    }

    void OnEnable()
    {
        UpdateAmmoText();
        ToggleAmmoText(true);
        isReloading = false;
        anim.SetBool("Reloading", false);       
    }

    private void OnDisable()
    {       
        ToggleAmmoText(false);      
    }

    void Update()
    {
        // Check for reload input and conditions
        if (Input.GetButtonDown("reload") && currentAmmo == 0 && maxAmmo >= 6 && !isReloading)
        {
            StartCoroutine(Reload());
            return;
        }

        // Check for shooting input and conditions
        if (!isReloading && Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !isRecoilPlaying)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            StartCoroutine(Recoil());
        }
    }

    // Logic for shooting
    void Shoot()
    {
        // Check for available ammo
        if (currentAmmo <= 0)
        {
            noAmmoSound.Play();
            gun.GetComponent<Animator>().Play("Idle 2");
            return;
        }

        // Play visual and audio effects
        muzzleFlash.Play();
        weaponSound.Play();

        currentAmmo--;
        UpdateAmmoText();

        // Create casing and impact effects
        GameObject casing = Instantiate(bulletCasing, casinglocation.position, casinglocation.rotation);
        Destroy(casing, 2f);

        // Apply force to the casing
        casing.GetComponent<Rigidbody>().AddForce(transform.forward * -100);

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

    //Toggle the visibility of the ammo UI text
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
        gun.GetComponent<Animator>().Play("Recoil");
        yield return new WaitForSeconds(1f);
        gun.GetComponent<Animator>().Play("Idle");
        isRecoilPlaying = false;
    }

    // Coroutine for reloading
    IEnumerator Reload()
    {
        // Disable switching during reload
        reloadSound.Play();

        // Set reloading flag and trigger animation
        isReloading = true;
        anim.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);

        // Reset animation and update ammo
        anim.SetBool("Reloading", false);
        maxAmmo -= 6;
        currentAmmo = 6;

        if (maxAmmo < 0) // Ensure maxAmmo doesn't go below 0
            maxAmmo = 0;

        UpdateAmmoText();

        // Reset reloading flag and enable switching
        isReloading = false;
    }

}
