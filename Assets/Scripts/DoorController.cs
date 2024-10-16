using UnityEngine;
using TMPro; // Add this to use TextMeshPro

public class DoorController : MonoBehaviour
{
    [Tooltip("Reference to the player object to detect.")]
    public GameObject player;

    [Tooltip("The speed at which the door rotates.")]
    public float doorRotationSpeed = 1f; // Speed factor for Lerp (1 is normal speed)

    [Tooltip("The target rotation offset for the door when fully open.")]
    public Vector3 openRotationOffset; // Offset in degrees as a Vector3

    [Tooltip("The TextMeshPro UI element to display interaction instructions.")]
    public TextMeshProUGUI interactionText;

    private Quaternion initialRotation; // Initial rotation of the door
    private Quaternion targetRotation;  // Target rotation for the door
    private bool playerIsInside = false; // Tracks if player is inside the collider
    private bool isDoorOpen = false;     // Tracks if the door is fully open or closed
    private float lerpTime = 0f;         // Track time for the Lerp process

    private void Start()
    {
        // Store the initial rotation of the door
        initialRotation = transform.rotation;

        // Calculate the target rotation based on the offset
        targetRotation = initialRotation * Quaternion.Euler(openRotationOffset);

        // Hide the interaction text at the start
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerIsInside = true;

            // Show interaction text based on door state
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = isDoorOpen ? "Press E to Close" : "Press E to Open";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerIsInside = false;

            // Hide interaction text when the player exits the trigger area
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // If the player is inside the trigger and presses E, toggle door state
        if (playerIsInside && Input.GetKeyDown(KeyCode.E))
        {
            lerpTime = 0f; // Reset lerp time before starting
            if (isDoorOpen)
            {
                CloseDoor(); // Close the door if it's open
            }
            else
            {
                OpenDoor(); // Open the door if it's closed
            }
        }
    }

    void OpenDoor()
    {
        StartCoroutine(LerpRotation(targetRotation, "Press E to Close"));
        isDoorOpen = true; // Update state to open
    }

    void CloseDoor()
    {
        StartCoroutine(LerpRotation(initialRotation, "Press E to Open"));
        isDoorOpen = false; // Update state to closed
    }

    System.Collections.IEnumerator LerpRotation(Quaternion targetRotation, string interactionTextMessage)
    {
        // Continuously Lerp the door to the target rotation
        while (lerpTime < 1f)
        {
            lerpTime += Time.deltaTime * doorRotationSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpTime);
            yield return null; // Wait for the next frame
        }

        // Update interaction text when the lerp finishes
        if (playerIsInside && interactionText != null)
        {
            interactionText.text = interactionTextMessage;
        }
    }
}