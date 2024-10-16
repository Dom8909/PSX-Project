using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    // Handles movement input
    public Vector3 GetMovementInput(Transform cameraTransform)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get the movement direction relative to the camera
        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        moveDirection.y = 0; // Only allow horizontal movement
        return moveDirection.normalized;
    }

    // Handles running input
    public bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift); // Check if the player is running
    }

    // Handles jumping input
    public bool JumpButtonPressed()
    {
        return Input.GetButtonDown("Jump");
    }

    // Handles continuous jump press
    public bool IsJumpHeld()
    {
        return Input.GetButton("Jump");
    }
}