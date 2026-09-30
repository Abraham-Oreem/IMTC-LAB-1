using UnityEngine;
using DG.Tweening;

public class RotationEffect : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float duration = 0.5f;

    public bool Interact()
    {
        target
            .DORotate(
                target.eulerAngles + new Vector3(0f, rotationAmount, 0f),
                duration
            )
            .SetEase(Ease.OutBack);

        return true;
    }
}