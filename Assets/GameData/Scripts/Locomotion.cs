using UnityEngine;

public class Locomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform head;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 90f;

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector2 joystick = GameManager.Instance.Input.LeftJoystick;

        Vector3 forward = head.forward;
        Vector3 right = head.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement =
            (forward * joystick.y) +
            (right * joystick.x);

        xrOrigin.position += movement * moveSpeed * Time.deltaTime;
    }

    private void Rotate()
{
    Vector2 joystick = GameManager.Instance.Input.RightJoystick;

    if (Mathf.Abs(joystick.x) < 0.1f) return;

    float rotation = joystick.x * rotationSpeed * Time.deltaTime;

    xrOrigin.RotateAround(head.position, Vector3.up, rotation);
}
}