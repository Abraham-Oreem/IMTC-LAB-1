using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Debug UI")]
    [SerializeField] private TMP_Text inputDebugText;

    public VRInputManager Input { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        InputActionMap vrMap = inputActions.FindActionMap("Controllers", true);

        Input = new VRInputManager(vrMap);
    }


    private void OnEnable()
    {
        if (inputActions != null)
        {
            InputActionMap vrMap = inputActions.FindActionMap("Controllers", true);
            vrMap.Enable();
        }
    }


    private void OnDisable()
    {
        if (inputActions != null)
        {
            InputActionMap vrMap = inputActions.FindActionMap("Controllers", true);
            vrMap.Disable();
        }
    }

    void Update()
    {
        TestControllerInputs();
    }

    private void TestControllerInputs()
    {
        if (Input == null || inputDebugText == null)
        {
            Debug.Log("Empty");
            return;
        }

        Vector3 rightPosition = Input.RightPosition;
        Vector3 leftPosition = Input.LeftPosition;

        Vector3 rightRotation = Input.RightRotation.eulerAngles;
        Vector3 leftRotation = Input.LeftRotation.eulerAngles;

        inputDebugText.text =
            "===== RIGHT CONTROLLER =====\n" +
            $"Position: {rightPosition}\n" +
            $"Rotation: {rightRotation}\n" +
            $"Joystick: {Input.RightJoystick}\n" +
            $"Trigger: {Input.RightTrigger:F2}\n" +
            $"Trigger Button: {Input.RightTriggerPressed}\n" +
            $"A Button: {Input.RightPrimaryPressed}\n" +
            $"B Button: {Input.RightSecondaryPressed}\n\n" +

            "===== LEFT CONTROLLER =====\n" +
            $"Position: {leftPosition}\n" +
            $"Rotation: {leftRotation}\n" +
            $"Joystick: {Input.LeftJoystick}\n" +
            $"Trigger: {Input.LeftTrigger:F2}\n" +
            $"Trigger Button: {Input.LeftTriggerPressed}\n" +
            $"X Button: {Input.LeftPrimaryPressed}\n" +
            $"Y Button: {Input.LeftSecondaryPressed}";
    }
}