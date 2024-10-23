using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 8.0f;
    public float airControlFactor = 0.6f; // Reduced control when jumping

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 1.5f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpGroundCheckDelay = 0.1f; // Add a small delay after jumping before checking if grounded
    private float jumpTimer;

    private bool isGrounded;
    private bool isJumping;
    private Vector3 velocity;

    [Header("References")]
    public Transform cameraTransform;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (groundCheck == null)
        {
            Debug.LogError("Ground check reference is missing.");
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleGravity();
    }

    private void HandleMovement()
    {
        // Ground check logic with delay after jump
        if (jumpTimer <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundDistance))
            {
                isGrounded = true;
                if (isJumping) // End the jump when grounded
                {
                    isJumping = false;
                }
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate camera-relative direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f; // Flatten the forward vector on the Y-axis to prevent unintended vertical movement
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Movement direction based on camera orientation
        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;

        // Running and Walking
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        // Move the player based on input, only if there is significant movement
        if (moveDirection.magnitude >= 0.1f)
        {
            if (!isJumping) // Only move if not jumping
            {
                characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
            }
            else // Allow limited air control while jumping
            {
                characterController.Move(moveDirection.normalized * speed * airControlFactor * Time.deltaTime);
            }
        }

        // Jumping logic
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            isJumping = true;
            jumpTimer = jumpGroundCheckDelay;  // Start delay timer to prevent immediate ground check
        }
    }

    private void HandleGravity()
    {
        // Apply gravity when jumping or falling
        if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime; // Floaty jump effect
        }
        else if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime; // Fast falling
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Apply gravity to the character controller
        characterController.Move(velocity * Time.deltaTime);
    }

    // Accessor for the animation controller to check if the player is grounded
    public bool IsGrounded()
    {
        return isGrounded;
    }

    // Accessor for the animation controller to check if the player is jumping
    public bool IsJumping()
    {
        return isJumping;
    }

    public void StartJump()
    {
        // Start jump logic
        Debug.Log("Physics: Jump started");
        isJumping = true;
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Launch the player upward
    }

    public void EndJump()
    {
        // End jump logic
        Debug.Log("Physics: Jump ended");
        isJumping = false;
        velocity.y = 0; // Reset vertical velocity
    }
}