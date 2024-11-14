using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
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

    // ControllerControls reference (generated class from Input Actions asset)
    private ControllerControls controls;

    // Input values
    private Vector2 rightStickInput;
    private Vector2 leftStickInput;  // For left thumbstick input
    private float leftTriggerInput;
    private float rightTriggerInput;

    // Slider speed
    public float sliderSpeed = 1.0f;

    private void Awake()
    {
        // Initialize input actions
        controls = new ControllerControls();

        // Bind actions
        controls.Controller.RightStick.performed += ctx => rightStickInput = ctx.ReadValue<Vector2>();
        controls.Controller.LeftStick.performed += ctx => leftStickInput = ctx.ReadValue<Vector2>();  // Bind the left thumbstick input
        controls.Controller.LeftTrigger.performed += ctx => leftTriggerInput = ctx.ReadValue<float>();
        controls.Controller.RightTrigger.performed += ctx => rightTriggerInput = ctx.ReadValue<float>();
    }

    private void OnEnable()
    {
        // Enable the controls when the game starts
        controls.Enable();
    }

    private void OnDisable()
    {
        // Disable the controls when the game is paused or stops
        controls.Disable();
    }

    private void Update()
    {
        // Stop Y movement when the left trigger is pressed
        if (leftTriggerInput > 0.1f)
        {
            // Left trigger pressed, so no movement for the Y slider
            ySlider.value += 0; // No change in Y slider
        }
        else if (leftStickInput != Vector2.zero) // Only update if there's input on the left stick
        {
            // Left trigger not pressed, Y slider responds to left thumbstick's horizontal movement
            ySlider.value += leftStickInput.x * sliderSpeed * Time.deltaTime;
        }

        // Move X slider only when left thumbstick has input (horizontal movement)
        if (leftTriggerInput <= 0.1f && leftStickInput.x != 0) // Only update if there's input
        {
            xSlider.value += leftStickInput.y * sliderSpeed * Time.deltaTime; // Vertical movement of left stick controls X slider
        }

        // Move Z slider when left trigger is pressed and left stick moves vertically
        if (leftTriggerInput > 0.1f && leftStickInput.y != 0) // Only update if there's input
        {
            zSlider.value += leftStickInput.y * sliderSpeed * Time.deltaTime;
        }

        // Move RY slider unless right trigger is pressed
        if (rightTriggerInput <= 0.1f && rightStickInput != Vector2.zero) // Only move RY if right trigger is NOT pressed and there's input
        {
            rySlider.value += rightStickInput.y * sliderSpeed * Time.deltaTime;
        }

        // Move RX slider only if the right trigger is NOT pressed (meaning RZ is not being controlled)
        if (rightTriggerInput <= 0.1f && rightStickInput.x != 0) // Only update RX if right trigger is NOT pressed
        {
            rxSlider.value += rightStickInput.x * sliderSpeed * Time.deltaTime;
        }

        // Move RZ slider when right trigger is pressed
        if (rightTriggerInput > 0.1f && rightStickInput.x != 0) // Only update RZ if right trigger is pressed and there's input
        {
            rzSlider.value += rightStickInput.x * sliderSpeed * Time.deltaTime;
        }

        // Update TextMeshPro text fields to show current slider values
        xSliderText.text = xSlider.value.ToString("F2");
        ySliderText.text = ySlider.value.ToString("F2");
        zSliderText.text = zSlider.value.ToString("F2");
        rxSliderText.text = rxSlider.value.ToString("F2");
        rySliderText.text = rySlider.value.ToString("F2");
        rzSliderText.text = rzSlider.value.ToString("F2");
    }
}
