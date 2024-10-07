using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of character movement
    public float rotationSpeed = 700f; // Speed of camera rotation
    public Transform cameraTransform; // Reference to the player's camera

    private bool cursorLocked = true; // Cursor locked state

    private CharacterController controller;
    private float pitch = 0f; // Vertical rotation for looking up and down

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Set the initial cursor lock state
        LockCursor();
    }

    void Update()
    {
        // Toggle cursor lock/unlock with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }

        if (cursorLocked)
        {
            // Character movement based on camera direction
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
            moveDirection.y = 0; // Prevent upward/downward movement from affecting horizontal movement

            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Rotate player horizontally with the mouse
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);

            // Look up and down with the mouse
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch -= mouseY; // Invert mouseY to make it look natural
            pitch = Mathf.Clamp(pitch, -85f, 85f); // Limit vertical look

            cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f); // Rotate camera for looking up/down
        }
    }

    // Method to lock the cursor
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    // Method to unlock the cursor
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    // Method to toggle cursor lock/unlock state
    void ToggleCursorLock()
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