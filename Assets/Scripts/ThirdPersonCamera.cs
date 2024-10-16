using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Tooltip("Reference to the player object to follow.")]
    public Transform playerTransform; // Reference to the player

    [Tooltip("Distance from the player.")]
    public float distance = 5f; // Default distance of the camera from the player

    [Tooltip("Height of the camera above the player.")]
    public float height = 2f; // Height of the camera above the player

    [Tooltip("Speed of smooth camera movement.")]
    public float smoothSpeed = 0.125f; // Speed for smooth camera movement

    [Tooltip("Layers that the camera can collide with.")]
    public LayerMask collisionLayers; // Layers the camera should collide with

    [Tooltip("Distance buffer for collision detection.")]
    public float collisionBuffer = 0.2f; // Distance buffer for collision to avoid clipping

    [Tooltip("Minimum distance between the camera and player.")]
    public float minDistanceToPlayer = 1.5f; // Minimum distance to prevent camera from getting too close

    [Tooltip("Minimum height relative to the player that the camera should never go below.")]
    public float minCameraHeightOffset = 0.5f; // The camera will not go below player's feet

    [Tooltip("Collision smoothing speed.")]
    public float collisionSmoothingSpeed = 10f; // Smoothing speed for soft transitions during collisions

    [Tooltip("Minimum vertical rotation (pitch) angle.")]
    public float minYRotation = -30f; // Minimum Y (pitch) rotation

    [Tooltip("Maximum vertical rotation (pitch) angle.")]
    public float maxYRotation = 60f;  // Maximum Y (pitch) rotation

    private float pitch = 0f; // Vertical rotation for looking up and down
    private float currentX = 0f; // Horizontal rotation
    private bool cursorLocked = true; // Cursor lock state

    [Tooltip("Reference to MouseSensitivityController for sensitivity adjustments.")]
    public MouseSensitivityController sensitivityController; // Reference to MouseSensitivityController

    private void Start()
    {
        // Ensure sensitivityController is assigned
        if (sensitivityController == null)
        {
            sensitivityController = GetComponent<MouseSensitivityController>();

            if (sensitivityController == null)
            {
                Debug.LogError("MouseSensitivityController is not assigned or attached to the same GameObject.");
            }
        }

        LockCursor(); // Lock the cursor at the start of the game
    }

    private void Update()
    {
        if (sensitivityController == null) return;

        // Toggle cursor lock/unlock with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }

        if (cursorLocked)
        {
            // Rotate the camera based on mouse input with sensitivity applied
            float mouseX = Input.GetAxis("Mouse X") * sensitivityController.GetHorizontalSensitivity() * Time.deltaTime;
            currentX += mouseX; // Update the horizontal rotation

            float mouseY = Input.GetAxis("Mouse Y") * sensitivityController.GetVerticalSensitivity() * Time.deltaTime;
            pitch -= mouseY; // Invert mouseY to make it feel natural

            // Clamp the vertical rotation between minYRotation and maxYRotation
            pitch = Mathf.Clamp(pitch, minYRotation, maxYRotation);

            // Update camera position and rotation
            UpdateCamera();

            // Rotate the player based on camera direction
            RotatePlayer();
        }
    }

    // Method to update the camera's position and rotation
    private void UpdateCamera()
    {
        Vector3 direction = new Vector3(0, height, -distance); // Camera offset
        Quaternion rotation = Quaternion.Euler(pitch, currentX, 0);
        Vector3 desiredPosition = playerTransform.position + rotation * direction;

        // Handle camera collision detection and ensure the camera doesn't go below the player
        Vector3 adjustedPosition = HandleCameraCollision(desiredPosition);

        // Smooth camera movement to avoid sudden shifts
        transform.position = Vector3.Lerp(transform.position, adjustedPosition, smoothSpeed);
        transform.LookAt(playerTransform.position + Vector3.up * height); // Look at the player
    }

    // Method to rotate the player to face the camera direction
    private void RotatePlayer()
    {
        Vector3 cameraForward = new Vector3(transform.forward.x, 0, transform.forward.z); // Flatten the camera's forward direction
        playerTransform.forward = Vector3.Lerp(playerTransform.forward, cameraForward.normalized, Time.deltaTime * smoothSpeed); // Smoothly rotate player to face camera direction
    }

    // Method to handle camera collision detection and ensure the camera stays above the player
    private Vector3 HandleCameraCollision(Vector3 desiredPosition)
    {
        RaycastHit hit;
        Vector3 directionToCamera = desiredPosition - playerTransform.position;
        float maxDistance = directionToCamera.magnitude;

        // SphereCast to detect collision with obstacles
        if (Physics.SphereCast(playerTransform.position, collisionBuffer, directionToCamera, out hit, maxDistance, collisionLayers))
        {
            // If collision detected, move the camera to the hit point, slightly offset by the buffer
            Vector3 collisionAdjustedPosition = hit.point + hit.normal * collisionBuffer;

            // Check if the collision is forcing the camera below the player's feet
            float minCameraHeight = playerTransform.position.y + minCameraHeightOffset;
            if (collisionAdjustedPosition.y < minCameraHeight)
            {
                // Adjust the camera position to stay above the minimum height
                collisionAdjustedPosition.y = minCameraHeight;
            }

            // If too close to the player, gently push the camera back
            float distanceToPlayer = Vector3.Distance(hit.point, playerTransform.position);
            if (distanceToPlayer < minDistanceToPlayer)
            {
                collisionAdjustedPosition = playerTransform.position - directionToCamera.normalized * minDistanceToPlayer;
            }

            // Smooth transition to collision position using Lerp
            return Vector3.Lerp(transform.position, collisionAdjustedPosition, Time.deltaTime * collisionSmoothingSpeed);
        }

        // If no collision, return the desired position, but check if it's below the player
        if (desiredPosition.y < playerTransform.position.y + minCameraHeightOffset)
        {
            // If the camera is below the player, move it above the player's minimum height
            desiredPosition.y = playerTransform.position.y + minCameraHeightOffset;
        }

        return desiredPosition;
    }

    // Method to lock the cursor
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    // Method to unlock the cursor
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    // Method to toggle cursor lock/unlock state
    private void ToggleCursorLock()
    {
        if (cursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }
}