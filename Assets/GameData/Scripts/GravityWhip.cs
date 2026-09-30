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
    [SerializeField] private int velocitySamples = 6;

    private Rigidbody grabbedRigidbody;
    private Tween grabTween;
    private bool isHoldingObject;

    private Vector3[] positionHistory;
    private float[] timeHistory;
    private int historyIndex;
    private int historyCount;

    private void Awake()
    {
        positionHistory = new Vector3[velocitySamples];
        timeHistory = new float[velocitySamples];
    }

    private void Update()
    {
        RecordPosition();

        if (!isHoldingObject)
            return;

        bool isStillFlyingToHand = grabTween != null && grabTween.IsActive() && grabTween.IsPlaying();

        if (!isStillFlyingToHand)
            grabbedRigidbody.MovePosition(grabOffset.position);
    }

    private void RecordPosition()
    {
        positionHistory[historyIndex] = grabOffset.position;
        timeHistory[historyIndex] = Time.time;

        historyIndex = (historyIndex + 1) % velocitySamples;

        if (historyCount < velocitySamples)
            historyCount++;
    }

    private Vector3 GetAverageVelocity()
    {
        if (historyCount < 2)
            return Vector3.zero;

        int newest = (historyIndex - 1 + velocitySamples) % velocitySamples;
        int oldest = (historyIndex - historyCount + velocitySamples) % velocitySamples;

        float timePassed = timeHistory[newest] - timeHistory[oldest];

        if (timePassed <= 0f)
            return Vector3.zero;

        return (positionHistory[newest] - positionHistory[oldest]) / timePassed;
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

        grabTween = grabbedRigidbody.transform
            .DOMove(grabOffset.position, grabDuration)
            .SetEase(Ease.OutQuad);
    }

    public void Release()
    {
        if (!isHoldingObject || grabbedRigidbody == null)
            return;

        Rigidbody releasedObject = grabbedRigidbody;

        grabTween?.Kill();
        grabTween = null;

        grabbedRigidbody = null;
        isHoldingObject = false;

        releasedObject.isKinematic = false;

        Vector3 throwVelocity = GetAverageVelocity() * throwMultiplier;

        throwVelocity = Vector3.ClampMagnitude(throwVelocity, maxThrowVelocity);

        releasedObject.linearVelocity = throwVelocity;
    }
}