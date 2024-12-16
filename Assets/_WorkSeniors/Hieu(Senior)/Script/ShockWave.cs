using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public float radius = 5f;         // Radius of the shockwave
    public float damage = 10f;        // Amount of damage dealt
    public float pushForce = 5f;      // How much force to apply to push enemies away
    public float shockwaveDuration = 0.5f; // Duration of the shockwave effect

    public LayerMask enemyLayer;      // Layer for enemy detection

    private void Start()
    {
        // Start the shockwave and destroy it after the duration.
        Destroy(gameObject, shockwaveDuration);
        ApplyShockwaveEffect();
    }

    void ApplyShockwaveEffect()
    {
        // Find all colliders in the shockwave radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            // Apply damage
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Calculate direction to push the enemy (from player to enemy)
            Vector2 pushDirection = (enemy.transform.position - transform.position).normalized;
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Apply the push force to the enemy's rigidbody
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
