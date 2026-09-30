using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform head;
    [SerializeField] private Transform aimFrom;

    [SerializeField] private LineRenderer line;

    [SerializeField] private GameObject marker;

    [SerializeField] private float launchSpeed = 8f;
    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private int maxPoints = 60;

    [SerializeField] private float timeStep = 0.04f;

    [SerializeField] private LayerMask teleportLayers = ~0;
    [SerializeField] private LayerMask blockingLayers = ~0;

    [Range(0f, 1f)]
    [SerializeField] private float minFlatness = 0.7f;

    [Header("Colors")]
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

    private bool isAiming = false;
    private bool hasValidTarget = false;
    private Vector3 targetPoint;
    private readonly List<Vector3> arcPoints = new List<Vector3>();

    private void Start()
    {
        HideArc();
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.Input == null) return;

        bool triggerHeld = GameManager.Instance.Input.LeftTriggerPressed;

        if (!isAiming && triggerHeld)
        {
            isAiming = true;
        }

        if (!isAiming) return;

        if (triggerHeld)
        {
            UpdateArc();
        }
        else
        {
            if (hasValidTarget)
                TeleportTo(targetPoint);

            isAiming = false;
            HideArc();
        }
    }

    private void UpdateArc()
    {
        arcPoints.Clear();
        hasValidTarget = false;

        Vector3 position = aimFrom.position;
        Vector3 velocity = aimFrom.forward * launchSpeed;

        arcPoints.Add(position);

        for (int i = 0; i < maxPoints; i++)
        {
            Vector3 nextPosition = position + velocity * timeStep;
            velocity += Vector3.down * gravity * timeStep;

            if (Physics.Linecast(position, nextPosition, out RaycastHit hit,
                     blockingLayers, QueryTriggerInteraction.Ignore))
            {
                arcPoints.Add(hit.point);
                bool isFlat = hit.normal.y >= minFlatness;
                bool isTeleportLayer = (teleportLayers.value & (1 << hit.collider.gameObject.layer)) != 0;
                hasValidTarget = isFlat && isTeleportLayer;
                targetPoint = hit.point;
                break;
            }

            arcPoints.Add(nextPosition);
            position = nextPosition;
        }

        DrawArc();
    }

    private void DrawArc()
    {
        line.enabled = true;
        line.positionCount = arcPoints.Count;
        line.SetPositions(arcPoints.ToArray());

        line.material.color = hasValidTarget ? validColor : invalidColor;

        marker.SetActive(hasValidTarget);
        if (hasValidTarget)
            marker.transform.position = targetPoint + Vector3.up * 0.01f;
    }

    private void HideArc()
    {
        if (line != null) line.enabled = false;
        if (marker != null) marker.SetActive(false);
    }

    private void TeleportTo(Vector3 target)
    {
        Vector3 feetPosition = new Vector3(head.position.x, xrOrigin.position.y, head.position.z);
        Vector3 offset = target - feetPosition;

        xrOrigin.position += offset;
    }
}