using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject shockwavePrefab; // The shockwave prefab
    public Transform shockwaveSpawnPoint; // Point where shockwave will spawn
    [SerializeField] private float cooldownDuration = 3f; // Cooldown time after triggering the shockwave
    private bool isOnCooldown = false; // Cooldown flag

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown)
        {
            TriggerShockwave(); // Trigger the shockwave if not on cooldown
        }
    }

    void TriggerShockwave()
    {
        // Start cooldown
        StartCoroutine(ShockwaveCooldown());

        // Instantiate the shockwave prefab at the spawn point
        GameObject shockwave = Instantiate(shockwavePrefab, shockwaveSpawnPoint.position, Quaternion.identity);

        // If the shockwave prefab has a particle system, destroy it after a certain duration (or let the prefab handle it)
        Destroy(shockwave, 1f); // Adjust the duration as needed
    }

    private IEnumerator ShockwaveCooldown()
    {
        isOnCooldown = true; // Set cooldown flag to true
        yield return new WaitForSeconds(cooldownDuration); // Wait for the cooldown duration
        isOnCooldown = false; // Cooldown is over
    }
}
