using UnityEngine;

public class ModularThirdPersonCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform playerTransform; // The player that the camera will follow
    public Transform cameraTransform; // Camera to follow player

    [Header("Camera Placement")]
    public float distanceFromPlayer = 5f; // Distance from the player
    public float heightAbovePlayer = 2f; // Height above the player
    public Vector3 offset = Vector3.zero; // Offset for finer control

    [Header("Camera Behavior")]
    public float followSpeed = 0.1f; // Speed at which the camera follows the player
    public float smoothRotationSpeed = 10f; // Smoothing speed for camera rotation
    public float maxPitchAngle = 60f; // Maximum vertical (pitch) rotation
    public float minPitchAngle = -20f; // Minimum vertical (pitch) rotation

    [Header("Player Rotation Settings")]
    public float playerRotationSpeed = 10f; // How fast the player rotates to face the camera direction

    [Header("Collision Settings")]
    public LayerMask collisionLayers; // Layers the camera should collide with
    public float collisionRadius = 0.3f; // Radius for collision detection
    public float collisionBuffer = 0.5f; // Buffer distance to prevent camera clipping

    [Header("Mouse Sensitivity")]
    public MouseSensitivityController sensitivityController; // Reference to mouse sensitivity script

    private float pitch = 0f; // Vertical rotation (pitch)
    private float yaw = 0f; // Horizontal rotation (yaw)

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (sensitivityController == null)
        {
            Debug.LogError("MouseSensitivityController is not assigned!");
        }

        LockCursor();
    }

    private void Update()
    {
        HandleCameraRotation();
        UpdateCameraPosition();
        RotatePlayerToCameraDirection();
    }

    // Handles the rotation of the camera around the player based on mouse input and sensitivity
    private void HandleCameraRotation()
    {
        // Get mouse input with sensitivity applied
        float mouseX = Input.GetAxis("Mouse X") * sensitivityController.GetHorizontalSensitivity() * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityController.GetVerticalSensitivity() * Time.deltaTime;

        // Adjust yaw and pitch
        yaw += mouseX;
        pitch -= mouseY;

        // Clamp pitch between min and max angles
        pitch = Mathf.Clamp(pitch, minPitchAngle, maxPitchAngle);
    }

    // Updates the camera's position and handles collision detection
    private void UpdateCameraPosition()
    {
        // Calculate desired camera position based on yaw, pitch, and distance
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = playerTransform.position - (rotation * Vector3.forward * distanceFromPlayer + new Vector3(0, -heightAbovePlayer, 0)) + offset;

        // Check for collisions and adjust camera position if necessary
        Vector3 finalPosition = HandleCameraCollisions(desiredPosition);

        // Smoothly move the camera to the final position
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, finalPosition, followSpeed);
        cameraTransform.LookAt(playerTransform.position + Vector3.up * heightAbovePlayer); // Look at the player
    }

    // Handles player rotation to face the direction the camera is facing
    private void RotatePlayerToCameraDirection()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Ignore vertical rotation

        if (cameraForward.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * playerRotationSpeed);
        }
    }

    // Handle camera collisions to prevent clipping through objects
    private Vector3 HandleCameraCollisions(Vector3 desiredPosition)
    {
        RaycastHit hit;
        Vector3 directionToCamera = desiredPosition - playerTransform.position;
        float maxDistance = directionToCamera.magnitude;

        // Perform a SphereCast to detect collisions
        if (Physics.SphereCast(playerTransform.position, collisionRadius, directionToCamera, out hit, maxDistance, collisionLayers))
        {
            // If we hit an object, move the camera closer to avoid clipping
            return hit.point + hit.normal * collisionBuffer;
        }

        // No collision detected, return the desired position
        return desiredPosition;
    }

    // Locks the cursor at the center of the screen
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Unlocks the cursor for menu interaction, etc.
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}