using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component missing on the player.");
        }
    }

    // Update movement animations based on player movement and running state
    public void UpdateMovementAnimations(bool isMoving, bool isRunning)
    {
        animator.SetBool("IsWalking", isMoving && !isRunning);  // Walking state
        animator.SetBool("IsRunning", isMoving && isRunning);    // Running state
    }

    // Trigger the jump animation
    public void TriggerJump()
    {
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsGrounded", false);  // Player is in the air, no longer grounded
    }

    // Handle landing (when player hits the ground)
    public void Land()
    {
        animator.SetBool("IsJumping", false);   // Stop jumping
        animator.SetBool("IsGrounded", true);   // Player is now grounded
    }

    // Set grounded state (when falling or jumping ends)
    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool("IsGrounded", isGrounded);
    }
}