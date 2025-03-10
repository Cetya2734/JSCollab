using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;

    // Flags to control different actions upon taking damage
    public bool animate;
    public bool replace;
    public bool destroy;

    [SerializeField] private float blinkIntensity;
    [SerializeField] private float blinkDuration;
    [SerializeField] private float blinkTimer;
    [SerializeField] private SkinnedMeshRenderer skinnedMesh;
    private Color originalColor; // Store the original color
    
    private void Start()
    {
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        
        if (skinnedMesh != null)
        {
            // Store original material color
            originalColor = skinnedMesh.material.color;
            skinnedMesh.material = new Material(skinnedMesh.material); // Ensure unique material instance
        }
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
            // Reset to original color when not blinking
            skinnedMesh.material.color = originalColor;
        }
    }
    

    public void TakeDamage(float amount, Vector3 hitPos)
    {
        health -= amount;
        ParticleSpawnManager.Instance.SpawnParticle(ParticleSpawnManager.ParticleType.Hit,hitPos);
        if (health <= 0f)
        {
            Destroy();
        }

        blinkTimer = blinkDuration;
    }


    void Destroy()
    {
        Destroy(gameObject);
        ParticleSpawnManager.Instance.SpawnParticle(ParticleSpawnManager.ParticleType.Death, transform.position);
    }

}
