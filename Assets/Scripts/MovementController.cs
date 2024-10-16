using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GravityObject))] // Ensure GravityObject is attached
public class MovementController : MonoBehaviour
{
    [Tooltip("The speed at which the player walks.")]
    public float walkSpeed = 5f;

    [Tooltip("The speed at which the player runs.")]
    public float runSpeed = 10f;

    [Tooltip("The force applied when jumping.")]
    public float jumpForce = 7f;

    [Tooltip("The speed at which the player rotates to align with the camera direction.")]
    public float rotationSpeed = 700f;

    private CharacterController characterController;
    private GravityObject gravityObject; // Reference to GravityObject for gravity control

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        gravityObject = GetComponent<GravityObject>();

        if (characterController == null)
            Debug.LogError("CharacterController component missing.");

        if (gravityObject == null)
            Debug.LogError("GravityObject component missing.");
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    // Handles movement and rotation based on input
    public void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        bool isRunning = Input.GetKey(KeyCode.LeftShift); // Check if running

        if (moveDirection.magnitude >= 0.1f)
        {
            // Determine the target angle based on the camera's forward direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Calculate movement direction relative to the camera
            Vector3 direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Move the player
            float speed = isRunning ? runSpeed : walkSpeed;
            characterController.Move(direction * speed * Time.deltaTime);
        }
    }

    // Handles jumping based on input
    public void HandleJump()
    {
        bool isGrounded = characterController.isGrounded;

        // Handle jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Set upward velocity for jumping using the gravity system
            Vector3 velocity = gravityObject.GetVelocity();
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y); // Using default gravity value
            gravityObject.SetVelocity(velocity); // Set the new velocity in GravityObject
        }
    }
}