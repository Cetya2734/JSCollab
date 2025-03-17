using System;
using KBCore.Refs;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;
    public float lightBulbHealth = 30f;
    public float lightBulbDamageMultiplier = 2f;
    public float vulnerableDamageMultiplier = 1.5f;
    private bool isLightBulbDestroyed = false;

    [Header("Visual Settings")]
    [SerializeField] private float blinkIntensity = 2f;
    [SerializeField] private float blinkDuration = 0.5f;
    private float blinkTimer;
    [SerializeField, Child] SkinnedMeshRenderer skinnedMesh;
    private Color originalColor;

    [SerializeField, Child] GameObject lightBulbObject; // Assign the light bulb GameObject in the inspector

    [SerializeField] private AudioSource burstSound;
    
    private Enemy enemy; // Reference to the Enemy script

    private void Start()
    {
        if (skinnedMesh != null)
        {
            originalColor = skinnedMesh.material.color;
            skinnedMesh.material = new Material(skinnedMesh.material);
        }
        
        enemy = GetComponent<Enemy>();
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntensity) + 1.0f;
            skinnedMesh.material.color = Color.red * intensity;
        }
        else
        {
            skinnedMesh.material.color = originalColor;
        }
    }

    public void TakeDamage(float amount, Vector3 hitPos, bool isLightBulbHit)
    {
        if (isLightBulbHit && !isLightBulbDestroyed)
        {
            float damageDealt = amount * lightBulbDamageMultiplier;
            lightBulbHealth -= damageDealt;
            health -= damageDealt;

            if (lightBulbHealth <= 0)
            {
                isLightBulbDestroyed = true;
                if (lightBulbObject != null)
                    lightBulbObject.SetActive(false); // Disable the light bulb
                ParticleSpawnManager.Instance.SpawnParticle(ParticleSpawnManager.ParticleType.Explosion, hitPos);
                burstSound.Play();
            }
        }
        else
        {
            float damage = amount * (isLightBulbDestroyed ? vulnerableDamageMultiplier : 1f);
            health -= damage;
        }
        
        // Trigger stagger if the enemy script is present
        if (enemy != null)
        {
            enemy.OnTakeDamage(hitPos);
        }

        ParticleSpawnManager.Instance.SpawnParticle(ParticleSpawnManager.ParticleType.Hit, hitPos);

        if (health <= 0f)
            Destroy();
        
        blinkTimer = blinkDuration;
    }

    void Destroy()
    {
        Destroy(gameObject);
        ParticleSpawnManager.Instance.SpawnParticle(ParticleSpawnManager.ParticleType.Death, transform.position);
    }
}