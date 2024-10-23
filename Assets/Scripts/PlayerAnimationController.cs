using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;  // Reference to the movement script

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // Link to the movement script

        if (animator == null)
        {
            Debug.LogError("Animator not found on the player.");
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovementWithCamera script not found.");
        }
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        // Grounded and Jumping animations
        bool isGrounded = playerMovement.IsGrounded();
        bool isJumping = playerMovement.IsJumping();

        // Set the Animator parameters
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsJumping", isJumping);

        // Walking and Running animations
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        animator.SetBool("IsWalking", isMoving && !isRunning);  // Trigger walking animation if moving but not running
        animator.SetBool("IsRunning", isMoving && isRunning);   // Trigger running animation if moving and running
    }

    // Event: Triggered at the start of the jump animation
    public void OnJumpStart()
    {
        Debug.Log("Jump started in animation");
        playerMovement.StartJump();
    }

    // Event: Triggered at the peak of the jump animation
    public void OnJumpPeak()
    {
        Debug.Log("Reached jump peak");
        // If you want to change gravity or any other behavior at the peak, you can modify it here
    }

    // Event: Triggered when the player lands
    public void OnJumpLand()
    {
        Debug.Log("Player landed via animation");
        playerMovement.EndJump();
    }
}