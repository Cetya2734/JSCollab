using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] float currentHealth;

    [Header("Heart Rate UI Settings")]
    [SerializeField] private Image heartRateImage; // The UI Image whose color will change
    [SerializeField] private float scrollSpeed = 20f; // Speed of texture scrolling (optional)

    [Header("Health Thresholds")]
    [SerializeField] private float healthyThreshold = 0.7f; // Above 70% health (green)
    [SerializeField] private float warningThreshold = 0.3f; // 30% to 70% health (yellow)
    // Below 30% is critical (red)

    private Material heartRateMaterial; // Optional, for scrolling effect

    private void Start()
    {
        currentHealth = maxHealth;

        if (heartRateImage != null)
        {
            heartRateMaterial = heartRateImage.material; // Cache material (if scrolling)
            UpdateHeartRateColor(); // Set initial color
        }
    }

    private void Update()
    {
        // Optional: Scroll the heart rate texture (if using a material)
        if (heartRateMaterial != null)
        {
            heartRateMaterial.mainTextureOffset += new Vector2(Time.deltaTime * (-scrollSpeed / 10), 0f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative health
        ScreenDamage.Instance.ShowDamage();

        UpdateHeartRateColor(); // Update color based on health

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 40;
            PlayerRespawn.Instance.Respawn();
        }
        else
        {
            PlayerRespawn.Instance.Respawn();
        }
    }

    private void UpdateHeartRateColor()
    {
        if (heartRateImage == null) return;

        float healthPercentage = currentHealth / maxHealth;
        Color newColor;

        // Set color based on health percentage
        if (healthPercentage > healthyThreshold)
        {
            newColor = new Color(0.5f, 1f, 0.5f); // Light green (healthy)
        }
        else if (healthPercentage > warningThreshold)
        {
            newColor = Color.yellow; // Yellow (warning)
        }
        else
        {
            newColor = Color.red; // Red (critical)
        }

        // Apply the new color
        heartRateImage.color = newColor;
    }
    
    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHeartRateColor(); // Update UI
        Debug.Log($"Health restored! Current: {currentHealth}/{maxHealth}");
    }
}