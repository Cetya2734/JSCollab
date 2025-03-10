using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CrosshairFeedback : MonoBehaviour
{
    public RectTransform top, bottom, left, right; // Assign UI squares in Inspector
    public float moveDistance = 20f; // How far the squares move outward when shooting
   // public float feedbackDuration = 0.5f; // Duration before resetting
    public float animationDuration = 0.1f; // Smooth animation duration

    public Image zoomCircle; // Assign a UI Image for zoom mode
    public float zoomDuration = 0.2f; // Smooth transition for zoom mode
    public float crosshairMoveInDuration = 0.15f; // How fast squares move in for zoom effect

    private Vector3 topStart, bottomStart, leftStart, rightStart;
    private bool isZooming = false;
    
    public float rateOfFire = 2f; // Default: 2 shots per second

    void Start()
    {
        // Store original positions
        topStart = top.localPosition;
        bottomStart = bottom.localPosition;
        leftStart = left.localPosition;
        rightStart = right.localPosition;

        // Ensure zoom circle is initially hidden
        zoomCircle.gameObject.SetActive(false);
        zoomCircle.rectTransform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right Mouse Button (Zoom)
        {
            ActivateZoomMode();
        }
        
        else if (Input.GetMouseButtonUp(1))
        {
            DeactivateZoomMode();
        }
    }

   public void AnimateCrosshair()
    {
        if (isZooming) return; // Disable shooting animation while zooming

        // Move outward
        top.DOLocalMoveY(topStart.y + moveDistance, animationDuration).SetEase(Ease.OutQuad);
        bottom.DOLocalMoveY(bottomStart.y - moveDistance, animationDuration).SetEase(Ease.OutQuad);
        left.DOLocalMoveX(leftStart.x - moveDistance, animationDuration).SetEase(Ease.OutQuad);
        right.DOLocalMoveX(rightStart.x + moveDistance, animationDuration).SetEase(Ease.OutQuad);

        float feedbackDuration = Mathf.Clamp(1f / rateOfFire, 0.05f, 0.5f); // Ensuring reasonable range
        // Return to original position after delay
        Invoke(nameof(ResetCrosshair), feedbackDuration);
    }

    void ResetCrosshair()
    {
        top.DOLocalMoveY(topStart.y, animationDuration).SetEase(Ease.OutQuad);
        bottom.DOLocalMoveY(bottomStart.y, animationDuration).SetEase(Ease.OutQuad);
        left.DOLocalMoveX(leftStart.x, animationDuration).SetEase(Ease.OutQuad);
        right.DOLocalMoveX(rightStart.x, animationDuration).SetEase(Ease.OutQuad);
    }

    void ActivateZoomMode()
    {
        if (isZooming) return;
        isZooming = true;

        // Move crosshair squares toward the center
        top.DOLocalMoveY(0, crosshairMoveInDuration).SetEase(Ease.InQuad);
        bottom.DOLocalMoveY(0, crosshairMoveInDuration).SetEase(Ease.InQuad);
        left.DOLocalMoveX(0, crosshairMoveInDuration).SetEase(Ease.InQuad);
        right.DOLocalMoveX(0, crosshairMoveInDuration).SetEase(Ease.InQuad);

        // Fade out squares
        top.GetComponent<Image>().DOFade(0, crosshairMoveInDuration);
        bottom.GetComponent<Image>().DOFade(0, crosshairMoveInDuration);
        left.GetComponent<Image>().DOFade(0, crosshairMoveInDuration);
        right.GetComponent<Image>().DOFade(0, crosshairMoveInDuration).OnComplete(() =>
        {
            // Hide crosshair squares completely after animation
            top.gameObject.SetActive(false);
            bottom.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
        });

        // Show zoom circle and scale up
        zoomCircle.gameObject.SetActive(true);
        zoomCircle.rectTransform.DOScale(Vector3.one, zoomDuration).SetEase(Ease.OutQuad);
    }

    void DeactivateZoomMode()
    {
        if (!isZooming) return;
        isZooming = false;

       // Shrink zoom circle and hide
        zoomCircle.rectTransform.DOScale(Vector3.zero, zoomDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            zoomCircle.gameObject.SetActive(false);
        });
        

        // Reset crosshair squares and make them visible again
        top.gameObject.SetActive(true);
        bottom.gameObject.SetActive(true);
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);

        // Reset opacity
        top.GetComponent<Image>().DOFade(1, animationDuration);
        bottom.GetComponent<Image>().DOFade(1, animationDuration);
        left.GetComponent<Image>().DOFade(1, animationDuration);
        right.GetComponent<Image>().DOFade(1, animationDuration);

        // Move squares back to original positions
        top.DOLocalMoveY(topStart.y, crosshairMoveInDuration).SetEase(Ease.OutQuad);
        bottom.DOLocalMoveY(bottomStart.y, crosshairMoveInDuration).SetEase(Ease.OutQuad);
        left.DOLocalMoveX(leftStart.x, crosshairMoveInDuration).SetEase(Ease.OutQuad);
        right.DOLocalMoveX(rightStart.x, crosshairMoveInDuration).SetEase(Ease.OutQuad);
    }
    
    public void SetRateOfFire(float newRate)
    {
        rateOfFire = newRate;
    }
}
