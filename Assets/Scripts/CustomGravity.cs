using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [Tooltip("The base force of gravity. A negative value simulates Earth-like gravity (e.g., -9.81).")]
    public float gravityForce = -9.81f;

    private Vector3 velocity; // The current vertical velocity of the object

    // Apply gravity to the object based on its weight
    public void ApplyGravity(bool isGrounded, float weightMultiplier)
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset downward velocity when grounded
        }

        // Apply gravity each frame, factoring in the weightMultiplier
        velocity.y += gravityForce * weightMultiplier * Time.deltaTime;
    }

    // Get current velocity (useful for movement scripts)
    public Vector3 GetVelocity()
    {
        return velocity;
    }

    // Set velocity (used for jumping or setting custom velocities)
    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    // Reset the vertical velocity
    public void ResetVelocity()
    {
        velocity.y = 0f;
    }
}