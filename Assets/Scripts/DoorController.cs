using TMPro;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Transform doorTransform;      // The transform of the door to be rotated
    public Vector3 openRotationOffset;   // The amount to rotate the door (e.g., (0, 90, 0) for 90 degrees on Y-axis)
    public float openSpeed = 2f;         // Speed of opening/closing
    public bool isOpen = false;          // Track if the door is open or closed
    public InteractText HUDText;

    private Quaternion closedRotation;   // Store the initial rotation of the door
    private Quaternion targetRotation;   // The target rotation (open or closed)
    private bool isMoving = false;       // Track if the door is in the middle of opening/closing

    void Start()
    {
        // Set the closed rotation as the door's initial local rotation
        closedRotation = doorTransform.localRotation;
        targetRotation = closedRotation;
    }

    public void Interact()
    {
        if (isMoving == false)
        {
            if (isOpen == true)
            {
                HUDText.UpdateText("Close Door");
            }
            else
            {
                HUDText.UpdateText("Open Door");
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractedWithDoor();
        }
    }

    private void InteractedWithDoor()
    {
        // Trigger the door open/close when interacted with
        if (!isMoving)
        {
            if (isOpen)
            {
                // Set the target rotation to the closed position
                targetRotation = closedRotation;
            }
            else
            {
                // Calculate the new target rotation by adding the offset to the current rotation
                targetRotation = doorTransform.localRotation * Quaternion.Euler(openRotationOffset);
            }

            isOpen = !isOpen; // Toggle door state
            StartCoroutine(RotateDoor(targetRotation)); // Start rotating the door
        }
    }

    System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isMoving = true; // The door is now rotating
        float elapsedTime = 0f;
        Quaternion startingRotation = doorTransform.localRotation;

        HUDText.UpdateText("");

        while (elapsedTime < 1f)
        {
            // Lerp from the current rotation to the target rotation
            doorTransform.localRotation = Quaternion.Lerp(startingRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * openSpeed; // Adjust the speed of rotation
            yield return null;
        }

        // Ensure final rotation is reached
        doorTransform.localRotation = targetRotation;
        isMoving = false; // Finished rotating
    }
}