using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private float interactionDistance = 10f;
    [SerializeField] private LayerMask interactionLayers;

    [Header("Ray Origin")]
    [SerializeField] private Transform rightController;

    [Header("Ray Visual")]
    [SerializeField] private LineRenderer interactionLine;

    [SerializeField] private GravityWhip gravityWhip;

    private bool interactionCompleted;

    private void Update()
    {
        bool triggerPressed =
            GameManager.Instance.Input.RightTriggerPressed;

        if (!triggerPressed)
        {
            gravityWhip.Release();
            interactionCompleted = false;
            HideRay();
            return;
        }

        if (interactionCompleted)
            return;

        UpdateRay();

        if (GameManager.Instance.Input.RightPrimaryPressed)
        {
            TryInteract();
        }
    }

    private void UpdateRay()
    {
        if (rightController == null || interactionLine == null)
            return;

        Vector3 start = rightController.position;
        Vector3 end =
            start + (-rightController.forward) * interactionDistance;

        if (Physics.Raycast(
            start,
            -rightController.forward,
            out RaycastHit hit,
            interactionDistance,
            interactionLayers))
        {
            end = hit.point;
        }

        interactionLine.positionCount = 2;
        interactionLine.SetPosition(0, start);
        interactionLine.SetPosition(1, end);

        interactionLine.enabled = true;
    }

    private void TryInteract()
    {
        Ray ray = new Ray(
            rightController.position,
            -rightController.forward
        );

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            interactionLayers))
        {
            return;
        }

        IGravityWhippable whippable =
            hit.collider.GetComponent<IGravityWhippable>();

        if (whippable != null && whippable.CanBeWhipped())
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                gravityWhip.Grab(rb);

                interactionCompleted = true;
                HideRay();
            }
        }

        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    private void HideRay()
    {
        if (interactionLine != null)
        {
            interactionLine.enabled = false;
        }
    }
}