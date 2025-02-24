using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaScript : MonoBehaviour
{

    public float damagePerSecond = 5f;  // Damage per second while inside
    public float postExitDuration = 5f; // Damage continues for 5 sec after exit
    public float sinkSpeed = 2f;        // How fast the lava sinks
    public float sinkDepth = 5f;        // How far the lava should sink before disappearing

    private bool playerInside = false;
    private bool isSinking = false;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position; // Store the original position
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            StartCoroutine(DamagePlayer(other.GetComponent<PlayerHealth>()));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            StartCoroutine(PostExitDamage(other.GetComponent<PlayerHealth>()));
        }
    }

    IEnumerator DamagePlayer(PlayerHealth health)
    {
        while (playerInside && health != null)
        {
            health.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f); // Damage every second
        }
    }

    IEnumerator PostExitDamage(PlayerHealth health)
    {
        float elapsedTime = 0;
        while (elapsedTime < postExitDuration && health != null)
        {
            health.TakeDamage(damagePerSecond);
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isSinking)
        {
            StartCoroutine(SinkAndDisappear());
        }
    }

    IEnumerator SinkAndDisappear()
    {
        isSinking = true;
        float targetY = originalPosition.y - sinkDepth; // Calculate the final Y position

        while (transform.position.y > targetY)
        {
            transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
            yield return null;
        }

        Destroy(gameObject); // Remove the lava when it has fully sunk
    }
}