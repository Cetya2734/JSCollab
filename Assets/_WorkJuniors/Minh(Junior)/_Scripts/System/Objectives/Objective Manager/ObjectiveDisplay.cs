// using Core.Gameplay.Objectives;
// using TMPro;
// using DG.Tweening;
// using UnityEngine;
//
// namespace UI.Game.Objectives
// {
//     public class ObjectiveDisplay : MonoBehaviour
//     {
//         [SerializeField] private TextMeshProUGUI _objectiveText;
//         [SerializeField] private float _fadeDuration = 0.5f;
//         [SerializeField] private Color _completedColor = Color.gray;
//
//         private Objective _objective;
//         private Color _originalColor;
//
//
//         public void Init(Objective objective)
//         {
//             _objective = objective;
//             _originalColor = _objectiveText.color;
//             
//             _objectiveText.text = objective.GetStatusText();
//             objective.OnValueChange += OnObjectiveValueChange;
//             objective.OnComplete += OnObjectiveComplete;
//         }
//         
//         private void UpdateText()
//         {
//             _objectiveText.text = _objective.GetStatusText();
//         }
//
//         private void OnObjectiveComplete()
//         {
//             // _objectiveText.text = $"<s>{_objective.GetStatusText()}</s>";
//             // _objectiveText.color = Color.gray; // Fade out completed objectives
//             
//             // Cross out text with strikethrough
//             _objectiveText.text = $"<s>{_objective.GetStatusText()}</s>";
//             
//             // Gray out + fade animation
//             _objectiveText.DOColor(_completedColor, _fadeDuration);
//             _objectiveText.DOFade(0.7f, _fadeDuration); // Slight transparency
//         }
//         
//         private void OnDestroy()
//         {
//             if (_objective != null)
//             {
//                 _objective.OnValueChange -= UpdateText;
//                 _objective.OnComplete -= OnObjectiveComplete;
//             }
//         }
//
//         private void OnObjectiveValueChange()
//         {
//             _objectiveText.text = _objective.GetStatusText();
//         }
//     }
// }

using Core.Gameplay.Objectives;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ObjectiveDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _objectiveText;
    
    [Header("Appearance Settings")]
    [SerializeField] private float _fadeInDuration = 0.5f;
    [SerializeField] private float _slideDistance = 20f;
    
    [Header("Animation Settings")]
    [SerializeField] private float _fadeOutDuration = 0.8f;
    [SerializeField] private float _strikethroughDelay = 0.3f;
    [SerializeField] private Color _completedColor = new Color(0.5f, 0.5f, 0.5f, 1f); // Gray with full opacity
    
    private Sequence _animationSequence;


    public void Init(Objective objective)
    {
        _objectiveText.text = objective.GetStatusText();
        _objectiveText.color = Color.white; // Reset to default

        PlayAppearAnimation();

        objective.OnComplete += () => PlayCompletionAnimation(objective);
    }
    
    
    private void PlayAppearAnimation()
    {
        _animationSequence?.Kill();
        
        // Store original position
        Vector3 originalPos = _objectiveText.rectTransform.localPosition;
        
        // Start position (slightly below)
        _objectiveText.rectTransform.localPosition = originalPos - new Vector3(0, _slideDistance, 0);
        
        _animationSequence = DOTween.Sequence()
            // Fade in + slide up simultaneously
            .Append(_objectiveText.DOFade(1f, _fadeInDuration))
            .Join(_objectiveText.rectTransform.DOLocalMoveY(originalPos.y, _fadeInDuration))
            .SetEase(Ease.OutQuad);
    }


    private void PlayCompletionAnimation(Objective objective)
    {
        // Kill any existing animation to prevent conflicts
        _animationSequence?.Kill();

        _animationSequence = DOTween.Sequence()
            // Step 1: Apply strikethrough immediately
            .AppendCallback(() => {
                _objectiveText.text = $"<s>{objective.GetStatusText()}</s>";
            })
            // Step 2: Wait briefly before fading
            .AppendInterval(_strikethroughDelay)
            // Step 3: Gray out and fade to zero opacity
            .Append(_objectiveText.DOColor(_completedColor, 0.2f))
            .Join(_objectiveText.DOFade(0f, _fadeOutDuration))
            // Step 4: Optional - disable or destroy after animation
            .OnComplete(() => {
                gameObject.SetActive(false); // or Destroy(gameObject);
            });
    }

    private void OnDestroy()
    {
        _animationSequence?.Kill();
    }
}