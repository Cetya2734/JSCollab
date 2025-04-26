// // using UnityEngine;
// //
// // public class PlayerHealth : MonoBehaviour
// // {
// //     public float maxHealth = 100f;
// //     private float currentHealth;
// //
// //     void Start()
// //     {
// //         currentHealth = maxHealth;
// //     }
// //
// //     public void TakeDamage(float amount)
// //     {
// //         currentHealth -= amount;
// //         ScreenDamage.Instance.ShowDamage();
// //         Debug.Log("Player took damage! Current health: " + currentHealth);
// //
// //         if (currentHealth <= 0)
// //         {
// //             Die();
// //         }
// //     }
// //
// //     void Die()
// //     {
// //         Debug.Log("Player died!");
// //     }
// // }
// //
// //
//
// using UnityEngine;
// using UnityEngine.UI;
//
// public class PlayerHealth : MonoBehaviour
// {
//     [Header("Health Settings")]
//     [SerializeField] private float maxHealth = 100f;
//     private float currentHealth;
//
//     [Header("Heart Rate UI Settings")]
//     [SerializeField] private Image heartRateImage; // Reference to the heart rate UI Image
//     [SerializeField] private float scrollSpeed = 20f; // Speed of texture scrolling
//     [SerializeField] private Sprite[] healthStatus; // Sprites for different health states
//
//     [Header("Health Thresholds")]
//     [SerializeField] private float healthyThreshold = 0.7f; // Above 70% health
//     [SerializeField] private float warningThreshold = 0.3f; // 30% to 70% health
//     // Below 30% is critical
//
//     private Material heartRateMaterial; // Material for scrolling effect
//
//     private void Start()
//     {
//         currentHealth = maxHealth;
//
//         if (heartRateImage != null)
//         {
//             //heartRateImage.color = new Color32(241, 247, 214, 255); // Initial color
//             heartRateMaterial = heartRateImage.material; // Cache material for scrolling
//             UpdateHeartRateUI(); // Set initial sprite
//         }
//     }
//
//     private void Update()
//     {
//         // Scroll the heart rate texture
//         if (heartRateMaterial != null)
//         {
//             heartRateMaterial.mainTextureOffset += new Vector2(Time.deltaTime * (-scrollSpeed / 10), 0f);
//             UpdateHeartRateUI(); // Set initial sprite
//         }
//     }
//
//     public void TakeDamage(float amount)
//     {
//         currentHealth -= amount;
//         currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative health
//         ScreenDamage.Instance.ShowDamage();
//
//         UpdateHeartRateUI(); // Update UI based on new health
//
//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//     }
//
//     private void Die()
//     {
//         Debug.Log("Player died!");
//     }
//
//     private void UpdateHeartRateUI()
//     {
//         if (heartRateImage == null || healthStatus == null || healthStatus.Length == 0) return;
//
//         float healthPercentage = currentHealth / maxHealth;
//         int spriteIndex = 0;
//         Color32 color;
//
//         // Determine sprite and color based on health
//         if (healthPercentage > healthyThreshold)
//         {
//             Debug.Log("Healthy");
//             spriteIndex = 0; // Healthy sprite
//             color = new Color32(0, 128, 0, 255); // Light green/white
//         }
//         else if (healthPercentage > warningThreshold)
//         {
//             Debug.Log("Low");
//             spriteIndex = healthStatus.Length > 1 ? 1 : 0; // Warning sprite
//             color = new Color32(255, 255, 0, 255); // Yellow
//         }
//         else
//         {
//             Debug.Log("Critical");
//             spriteIndex = healthStatus.Length > 2 ? 2 : (healthStatus.Length > 1 ? 1 : 0); // Critical sprite
//             color = new Color32(255, 0, 0, 255); // Red
//         }
//
//         // Update sprite and color
//         heartRateImage.sprite = healthStatus[spriteIndex];
//         heartRateImage.material.color = color;
//     }
// }


using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

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
        Debug.Log("Player died!");
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
}