using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float lifetime = 3f; // Time before the projectile is destroyed

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after lifetime
    }

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Implement collision logic (e.g., damage, destroy)
        Destroy(gameObject); // Destroy projectile on hit

    }
}

