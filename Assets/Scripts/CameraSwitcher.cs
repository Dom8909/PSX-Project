using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Tooltip("The default player camera.")]
    public Camera playerCamera;

    [Tooltip("The designated camera to switch to when the player enters a certain area.")]
    public Camera alternateCamera;

    [Tooltip("Collider that will trigger the camera switch.")]
    public Collider triggerZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchToAlternateCamera();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchToPlayerCamera();
        }
    }

    private void SwitchToAlternateCamera()
    {
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);

        if (alternateCamera != null)
            alternateCamera.gameObject.SetActive(true);
    }

    private void SwitchToPlayerCamera()
    {
        if (alternateCamera != null)
            alternateCamera.gameObject.SetActive(false);

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);
    }
}