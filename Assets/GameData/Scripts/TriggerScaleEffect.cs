using UnityEngine;
using DG.Tweening;

public class TriggerScaleEffect : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Scale Effect")]
    [SerializeField] private float scaleMultiplier = 1.3f;
    [SerializeField] private float scaleUpDuration = 0.15f;
    [SerializeField] private float scaleDownDuration = 0.2f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = target.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        IGravityWhippable interactable =
            other.GetComponent<IGravityWhippable>();

        if (interactable == null)
            return;

        target.DOKill();

        target
            .DOScaleY(originalScale.y * scaleMultiplier, scaleUpDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                target
                    .DOScale(originalScale, scaleDownDuration)
                    .SetEase(Ease.OutQuad);
            });
    }
}