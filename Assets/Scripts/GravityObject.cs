using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GravityObject : MonoBehaviour
{
    [Tooltip("Reference to the external CustomGravity component.")]
    public CustomGravity customGravity; // Reference to CustomGravity on another GameObject

    [Tooltip("Multiplier to control the object's weight. A higher value makes the object heavier.")]
    public float weightMultiplier = 1.0f; // Custom weight for each object

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController component missing.");
        }

        if (customGravity == null)
        {
            Debug.LogError("CustomGravity component is not assigned. Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (customGravity == null) return;

        // Check if the object is grounded
        bool isGrounded = characterController.isGrounded;

        // Apply gravity using the external CustomGravity reference with weightMultiplier
        customGravity.ApplyGravity(isGrounded, weightMultiplier);

        // Move the object using the current velocity
        characterController.Move(customGravity.GetVelocity() * Time.deltaTime);
    }

    // Expose GetVelocity to access the velocity from CustomGravity
    public Vector3 GetVelocity()
    {
        if (customGravity != null)
        {
            return customGravity.GetVelocity();
        }
        return Vector3.zero;
    }

    // Expose SetVelocity to modify the velocity in CustomGravity
    public void SetVelocity(Vector3 velocity)
    {
        if (customGravity != null)
        {
            customGravity.SetVelocity(velocity);
        }
    }
}