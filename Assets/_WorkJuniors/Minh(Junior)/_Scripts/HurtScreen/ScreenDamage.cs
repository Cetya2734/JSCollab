using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDamage : MonoBehaviour
{
    public static ScreenDamage Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Image bloodOverlay; // Your existing blood screen image
    [SerializeField] private float fadeInDuration = 0.1f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float maxOpacity = 0.8f; // Maximum opacity (0-1)

    private float currentOpacity = 0f;
    private bool isActive = false;
    private float fadeTimer = 0f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        // Initialize - start fully transparent
        if (bloodOverlay != null)
            bloodOverlay.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if (!isActive) return;

        fadeTimer += Time.deltaTime;

        if (fadeTimer <= fadeInDuration)
        {
            // Fade in
            currentOpacity = Mathf.Lerp(0, maxOpacity, fadeTimer / fadeInDuration);
        }
        else
        {
            // Fade out
            float fadeOutProgress = (fadeTimer - fadeInDuration) / fadeOutDuration;
            currentOpacity = Mathf.Lerp(maxOpacity, 0, fadeOutProgress);

            if (fadeOutProgress >= 1f)
                isActive = false;
        }

        UpdateOverlay();
    }
    
    public void ShowDamage()
    {
        if (bloodOverlay == null)
        {
            Debug.LogWarning("No blood overlay assigned to DamageFeedback!");
            return;
        }

        isActive = true;
        fadeTimer = 0f;
    }

    private void UpdateOverlay()
    {
        if (bloodOverlay != null)
        {
            Color currentColor = bloodOverlay.color;
            currentColor.a = currentOpacity;
            bloodOverlay.color = currentColor;
        }
    }
}