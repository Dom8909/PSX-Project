using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 1f; // Range of the raycast
    public GameObject interactText;

    void Update()
    {
        ShootRaycast();
    }

    void ShootRaycast()
    {
        // Ray starts from the camera and shoots forward
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the ray hits something within the interactRange
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // Get the object hit by the raycast
            GameObject hitObject = hit.collider.gameObject;

            // Check if the object has a component that implements IInteractable
            IInteractable interactable = hitObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("Interactable Object!");
                // If the object is interactable, call its Interact method
                interactable.Interact();
            }
            else
            {
                interactText.SetActive(false);
            }
        }
    }
}