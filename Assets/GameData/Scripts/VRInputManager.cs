using UnityEngine;
using UnityEngine.InputSystem;

public class VRInputManager
{
    private InputActionMap vrMap;

    private InputAction rightPosition;
    private InputAction rightRotation;
    private InputAction rightTrigger;
    private InputAction rightTriggerButton;
    private InputAction rightPrimaryButton;
    private InputAction rightSecondaryButton;
    private InputAction rightJoystick;

    private InputAction leftPosition;
    private InputAction leftRotation;
    private InputAction leftTrigger;
    private InputAction leftTriggerButton;
    private InputAction leftPrimaryButton;
    private InputAction leftSecondaryButton;
    private InputAction leftJoystick;


    public VRInputManager(InputActionMap actionMap)
    {
        vrMap = actionMap;

        rightPosition = vrMap.FindAction("RightPosition", true);
        rightRotation = vrMap.FindAction("RightRotation", true);
        rightTrigger = vrMap.FindAction("RightTrigger", true);
        rightTriggerButton = vrMap.FindAction("RightTriggerButton", true);
        rightPrimaryButton = vrMap.FindAction("RightPrimaryButton", true);
        rightSecondaryButton = vrMap.FindAction("RightSecondaryButton", true);
        rightJoystick = vrMap.FindAction("RightJoystick", true);

        leftPosition = vrMap.FindAction("LeftPosition", true);
        leftRotation = vrMap.FindAction("LeftRotation", true);
        leftTrigger = vrMap.FindAction("LeftTrigger", true);
        leftTriggerButton = vrMap.FindAction("LeftTriggerButton", true);
        leftPrimaryButton = vrMap.FindAction("LeftPrimaryButton", true);
        leftSecondaryButton = vrMap.FindAction("LeftSecondaryButton", true);
        leftJoystick = vrMap.FindAction("LeftJoystick", true);
    }



    public Vector3 RightPosition =>
        rightPosition.ReadValue<Vector3>();

    public Quaternion RightRotation =>
        rightRotation.ReadValue<Quaternion>();

    public float RightTrigger =>
        rightTrigger.ReadValue<float>();

    public bool RightTriggerPressed =>
        rightTriggerButton.IsPressed();

    public bool RightPrimaryPressed =>
        rightPrimaryButton.IsPressed();

    public bool RightSecondaryPressed =>
        rightSecondaryButton.IsPressed();

    public Vector2 RightJoystick =>
        rightJoystick.ReadValue<Vector2>();




    public Vector3 LeftPosition =>
        leftPosition.ReadValue<Vector3>();

    public Quaternion LeftRotation =>
        leftRotation.ReadValue<Quaternion>();

    public float LeftTrigger =>
        leftTrigger.ReadValue<float>();

    public bool LeftTriggerPressed =>
        leftTriggerButton.IsPressed();

    public bool LeftPrimaryPressed =>
        leftPrimaryButton.IsPressed();

    public bool LeftSecondaryPressed =>
        leftSecondaryButton.IsPressed();

    public Vector2 LeftJoystick =>
        leftJoystick.ReadValue<Vector2>();
}