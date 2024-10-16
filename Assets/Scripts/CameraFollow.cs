using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Player reference
    public float smoothSpeed = 0.125f; // Smoothing speed for camera follow
    public Vector3 offset; // Offset between the camera and the player

    private void LateUpdate()
    {
        // Get the target position based on player's position and the offset
        Vector3 desiredPosition = playerTransform.position + offset;

        // Smoothly move the camera towards the target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optionally, you can make the camera look at the player
        transform.LookAt(playerTransform);
    }
}