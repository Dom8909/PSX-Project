using UnityEngine;

public class MouseSensitivityController : MonoBehaviour
{
    // Public fields to adjust sensitivity in the Unity Inspector or via other scripts
    public float mouseSensitivityX = 100f; // Horizontal sensitivity
    public float mouseSensitivityY = 100f; // Vertical sensitivity

    // Method to get the horizontal sensitivity value
    public float GetHorizontalSensitivity()
    {
        return mouseSensitivityX;
    }

    // Method to get the vertical sensitivity value
    public float GetVerticalSensitivity()
    {
        return mouseSensitivityY;
    }

    // Optional: You can create methods to dynamically adjust sensitivity during runtime
    public void SetSensitivity(float sensitivityX, float sensitivityY)
    {
        mouseSensitivityX = sensitivityX;
        mouseSensitivityY = sensitivityY;
    }
}