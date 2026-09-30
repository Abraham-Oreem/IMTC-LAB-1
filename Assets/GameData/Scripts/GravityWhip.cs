using UnityEngine;
using DG.Tweening;

public class GravityWhip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform grabOffset;

    [Header("Grab")]
    [SerializeField] private float grabDuration = 0.25f;

    [Header("Throw")]
    [SerializeField] private float throwMultiplier = 1f;
    [SerializeField] private float maxThrowVelocity = 20f;

    private Rigidbody grabbedRigidbody;

    private Vector3 previousControllerPosition;
    private Vector3 controllerVelocity;

    private bool isHoldingObject;

    private void Update()
    {
        CalculateControllerVelocity();

        if (!isHoldingObject)
            return;

        grabbedRigidbody.MovePosition(grabOffset.position);
    }

    private void CalculateControllerVelocity()
    {
        Vector3 currentPosition =
            GameManager.Instance.Input.RightPosition;

        controllerVelocity =
            (currentPosition - previousControllerPosition) /
            Time.deltaTime;

        previousControllerPosition = currentPosition;
    }


    public void Grab(Rigidbody target)
    {
        if (target == null)
            return;

        if (isHoldingObject)
            return;

        grabbedRigidbody = target;

        grabbedRigidbody.isKinematic = true;

        isHoldingObject = true;

        grabbedRigidbody.transform
            .DOMove(grabOffset.position, grabDuration)
            .SetEase(Ease.OutQuad);
    }

    public void Release()
    {
        if (!isHoldingObject || grabbedRigidbody == null)
            return;

        Rigidbody releasedObject = grabbedRigidbody;

        grabbedRigidbody = null;
        isHoldingObject = false;

        releasedObject.isKinematic = false;

        Vector3 throwVelocity =
            controllerVelocity * throwMultiplier;

        throwVelocity = Vector3.ClampMagnitude(
            throwVelocity,
            maxThrowVelocity
        );

        releasedObject.linearVelocity = throwVelocity;
    }
}