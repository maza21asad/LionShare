using UnityEngine;
using DG.Tweening;

public class LionUI : MonoBehaviour
{
    [Header("Step Positions")]
    public RectTransform[] stepPositions;
    // Assign 0–9: cage position at index 0, step1 at index1, ...

    public RectTransform lionIcon;

    public float moveDuration = 0.5f;

    // Play open cage animation
    public void OpenCage()
    {
        // You can make cage door shake, glow, etc.
        // For now, simple scale punch
        lionIcon.DOPunchScale(Vector3.one * 0.1f, 0.3f, 5);
    }

    // Move lion to specific step visually
    public void MoveToStep(int step)
    {
        lionIcon.DOAnchorPos(stepPositions[step].anchoredPosition, moveDuration)
                .SetEase(Ease.OutBack);
    }

    // Round reset animation
    public void ResetPosition()
    {
        lionIcon.anchoredPosition = stepPositions[0].anchoredPosition;
    }
}
