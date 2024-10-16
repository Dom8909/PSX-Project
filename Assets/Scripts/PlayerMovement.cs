using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GravityObject))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The speed at which the player walks.")]
    public float walkSpeed = 5f;

    [Tooltip("The speed at which the player runs.")]
    public float runSpeed = 10f;

    [Tooltip("The force applied when jumping.")]
    public float jumpForce = 7f;

    [Tooltip("The speed at which the player rotates.")]
    public float rotationSpeed = 700f;

    private CharacterController characterController;
    private GravityObject gravityObject; // Reference to GravityObject for jump control
    private PlayerAnimationController animationController; // For handling animations
    private Vector3 moveDirection;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        gravityObject = GetComponent<GravityObject>();
        animationController = GetComponent<PlayerAnimationController>();

        if (characterController == null)
            Debug.LogError("CharacterController component missing.");

        if (gravityObject == null)
            Debug.LogError("GravityObject component missing.");

        if (animationController == null)
            Debug.LogError("PlayerAnimationController component missing.");
    }

    void Update()
    {
        HandleMovementAndJump();
        ApplyGravity();
        HandleAnimations();
    }

    // Combines movement and jump logic
    public void HandleMovementAndJump()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get direction based on player input
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        bool isRunning = Input.GetKey(KeyCode.LeftShift); // Check if running
        bool isMoving = inputDirection.magnitude >= 0.1f;  // Check if player is moving

        // Only rotate and move if there is input
        if (isMoving)
        {
            // Determine the target angle based on the camera's forward direction
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

            // Smoothly rotate the player using Quaternion.Slerp
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Calculate movement direction relative to the camera
            Vector3 direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Store the horizontal movement direction
            moveDirection = direction * (isRunning ? runSpeed : walkSpeed);
        }
        else
        {
            // No input: Stop horizontal movement but retain vertical velocity
            moveDirection.x = 0;
            moveDirection.z = 0;
        }

        // Handle jump input
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            // Trigger jump animation BEFORE applying upward velocity
            animationController.TriggerJump();

            // Set upward velocity for jumping using gravityObject
            Vector3 velocity = gravityObject.GetVelocity();
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y); // Using default gravity value
            gravityObject.SetVelocity(velocity);
        }
    }

    // Apply gravity to the player while keeping horizontal movement intact
    public void ApplyGravity()
    {
        Vector3 velocity = gravityObject.GetVelocity();

        // Combine the vertical velocity from gravity with the horizontal movement
        velocity.x = moveDirection.x;
        velocity.z = moveDirection.z;

        // Move the player using the CharacterController
        characterController.Move(velocity * Time.deltaTime);
    }

    // Handle animations for movement and jumping
    public void HandleAnimations()
    {
        bool isGrounded = characterController.isGrounded;

        // Update grounded state and movement animations
        animationController.SetGrounded(isGrounded);

        bool isMoving = moveDirection.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Update movement animations (walking or running)
        animationController.UpdateMovementAnimations(isMoving, isRunning);

        // If player lands after a jump, trigger landing animation
        if (isGrounded && gravityObject.GetVelocity().y < 0)
        {
            animationController.Land();
        }
    }
}