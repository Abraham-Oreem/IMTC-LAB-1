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

    [Header("Interaction UI")]
    [SerializeField] private GameObject interactionCanvas;
    [SerializeField] private Vector3 canvasOffset = new Vector3(0f, 0.5f, 0f);

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
        Vector3 direction = -rightController.forward;

        Vector3 end = start + direction * interactionDistance;

        if (Physics.Raycast(
            start,
            direction,
            out RaycastHit hit,
            interactionDistance,
            interactionLayers))
        {
            end = hit.point;

            IGravityWhippable whippable =
                hit.collider.GetComponent<IGravityWhippable>();

            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (whippable != null || interactable != null)
            {
                ShowInteractionCanvas(hit);
            }
            else
            {
                HideInteractionCanvas();
            }
        }
        else
        {
            HideInteractionCanvas();
        }

        interactionLine.positionCount = 2;
        interactionLine.SetPosition(0, start);
        interactionLine.SetPosition(1, end);

        interactionLine.enabled = true;
    }

    private void ShowInteractionCanvas(RaycastHit hit)
    {
        if (interactionCanvas == null)
            return;

        interactionCanvas.SetActive(true);

        interactionCanvas.transform.position =
            hit.collider.bounds.center + canvasOffset;

        Transform cameraTransform = Camera.main.transform;

        interactionCanvas.transform.LookAt(cameraTransform);

        interactionCanvas.transform.Rotate(0f, 180f, 0f);
    }

    private void HideInteractionCanvas()
    {
        if (interactionCanvas == null)
            return;

        interactionCanvas.SetActive(false);
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
            interactionCompleted = interactable.Interact();
            if (interactionCompleted) HideRay();
        }
    }

    private void HideRay()
    {
        if (interactionLine != null)
        {
            interactionLine.enabled = false;
        }

        HideInteractionCanvas();
    }
}