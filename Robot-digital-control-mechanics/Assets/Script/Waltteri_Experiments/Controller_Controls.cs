using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller_Controls : MonoBehaviour
{
    // References to the X, Y, Z, RX, RY, and RZ sliders
    public Slider xSlider;
    public Slider ySlider;
    public Slider zSlider;
    public Slider rxSlider;
    public Slider rySlider;
    public Slider rzSlider;

    // TextMeshPro references to display slider values
    public TMP_Text xSliderText;
    public TMP_Text ySliderText;
    public TMP_Text zSliderText;
    public TMP_Text rxSliderText;
    public TMP_Text rySliderText;
    public TMP_Text rzSliderText;

    // Reference to Robot Connection Buttons
    public Button connectButton;
    public Button disconnectButton;

    // Reference to the PIckUpTask script
    public PIckUpTask pickUpTask;

    // ControllerControls reference (generated class from Input Actions asset)
    private ControllerControls controls;

    // Input values
    private Vector2 rightStickInput = Vector2.zero;
    private Vector2 leftStickInput = Vector2.zero;
    private float leftTriggerInput = 0f;
    private float rightTriggerInput = 0f;

    // Slider speed
    public float sliderSpeed;

    // Connection state
    private bool isConnected = false;

    private void Awake()
    {
        // Initialize input actions
        controls = new ControllerControls();

        // Bind actions for input
        controls.Controller.RightStick.performed += ctx => rightStickInput = ctx.ReadValue<Vector2>();
        controls.Controller.RightStick.canceled += ctx => rightStickInput = Vector2.zero;

        controls.Controller.LeftStick.performed += ctx => leftStickInput = ctx.ReadValue<Vector2>();
        controls.Controller.LeftStick.canceled += ctx => leftStickInput = Vector2.zero;

        controls.Controller.LeftTrigger.performed += ctx => leftTriggerInput = ctx.ReadValue<float>();
        controls.Controller.LeftTrigger.canceled += ctx => leftTriggerInput = 0f;

        controls.Controller.RightTrigger.performed += ctx => rightTriggerInput = ctx.ReadValue<float>();
        controls.Controller.RightTrigger.canceled += ctx => rightTriggerInput = 0f;

        // Bind "PickUp" button action
        controls.Controller.PickUp.performed += ctx => HandlePickUpAction();

        // Bind "Connection" button action
        controls.Controller.Connection.performed += ctx => ToggleRobotConnection();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        // Control X and Y sliders only when the left trigger is NOT pressed
        if (leftTriggerInput <= 0.1f)
        {
            if (Mathf.Abs(leftStickInput.y) > 0.1f)
            {
                ySlider.value += leftStickInput.y * sliderSpeed * Time.deltaTime; // Up and down controls Y slider
            }

            if (Mathf.Abs(leftStickInput.x) > 0.1f)
            {
                xSlider.value += leftStickInput.x * sliderSpeed * Time.deltaTime; // Left and right controls X slider
            }
        }

        // Control Z slider only when the left trigger is pressed
        if (leftTriggerInput > 0.1f && Mathf.Abs(leftStickInput.y) > 0.1f)
        {
            zSlider.value += leftStickInput.y * sliderSpeed * Time.deltaTime; // Z slider responds to vertical movement
        }

        // Control RX and RY sliders only when the right trigger is NOT pressed
        if (rightTriggerInput <= 0.1f)
        {
            if (Mathf.Abs(rightStickInput.y) > 0.1f)
            {
                rySlider.value += rightStickInput.y * sliderSpeed * Time.deltaTime; // Up and down controls RY slider
            }

            if (Mathf.Abs(rightStickInput.x) > 0.1f)
            {
                rxSlider.value += rightStickInput.x * sliderSpeed * Time.deltaTime; // Left and right controls RX slider
            }
        }

        // Control RZ slider only when the right trigger is pressed
        if (rightTriggerInput > 0.1f && Mathf.Abs(rightStickInput.y) > 0.1f)
        {
            rzSlider.value += rightStickInput.y * sliderSpeed * Time.deltaTime; // RZ slider responds to vertical movement
        }

        // Update TextMeshPro text fields to show current slider values
        xSliderText.text = xSlider.value.ToString("F2");
        ySliderText.text = ySlider.value.ToString("F2");
        zSliderText.text = zSlider.value.ToString("F2");
        rxSliderText.text = rxSlider.value.ToString("F2");
        rySliderText.text = rySlider.value.ToString("F2");
        rzSliderText.text = rzSlider.value.ToString("F2");
    }

    // Toggle the robot connection
    private void ToggleRobotConnection()
    {
        if (connectButton == null || disconnectButton == null)
        {
            Debug.LogWarning("Connect or Disconnect button references are not set!");
            return;
        }

        if (isConnected)
        {
            // Perform disconnect action
            Debug.Log("Disconnecting robot...");
            disconnectButton.onClick.Invoke(); // Trigger the Disconnect button's onClick
            isConnected = false;
        }
        else
        {
            // Perform connect action
            Debug.Log("Connecting robot...");
            connectButton.onClick.Invoke(); // Trigger the Connect button's onClick
            isConnected = true;
        }
    }

    // Handle the "PickUp" button action
    private void HandlePickUpAction()
    {
        if (pickUpTask != null)
        {
            if (!pickUpTask.isHoldingCube && pickUpTask.canPickup)
            {
                pickUpTask.AttachCube();
            }
            else if (pickUpTask.isHoldingCube)
            {
                pickUpTask.DetachCube();
            }
        }
        else
        {
            Debug.LogWarning("PickUpTask reference is not assigned in the SliderController!");
        }
    }
}
