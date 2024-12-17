using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] private float radius = 5f;            // Radius of the shockwave
    [SerializeField] private float damage = 10f;           // Damage dealt to enemies
    [SerializeField] private float pushForce = 5f;         // Push force applied to enemies
    [SerializeField] private float shockwaveDuration = 0.5f; // Duration of shockwave effect

    [SerializeField] private ParticleSystem shockwaveParticle; // Particle prefab
    [SerializeField] private LayerMask enemyLayer;         // Enemy detection layer


    private void Start()
    {
        TriggerShockWave();
    }

    private void TriggerShockWave()
    {

        // Spawn the particle effect
        if (shockwaveParticle != null)
        {
            ParticleSystem spawnedParticle = Instantiate(shockwaveParticle, transform.position, Quaternion.identity);
            Destroy(spawnedParticle.gameObject, shockwaveDuration); // Destroy the particle after its duration
        }

        // Apply shockwave effect
        ApplyShockwaveEffect();

    }

    private void ApplyShockwaveEffect()
    {
        // Find all enemies within the radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            // Damage enemies
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Apply push force to enemies
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 pushDirection = (enemy.transform.position - transform.position).normalized;
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Debug visual for shockwave radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
