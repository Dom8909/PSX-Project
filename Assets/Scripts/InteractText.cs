using UnityEngine;
using TMPro; // Import for TextMeshPro

public class InteractText : MonoBehaviour
{
    public GameObject TargetObject;
    public TextMeshProUGUI interactTextMeshPro; // Reference to the TextMeshPro object

    void Start()
    {
        TargetObject.SetActive(false);
    }

    public void UpdateText(string newText)
    {
        if (interactTextMeshPro != null)
        {
            interactTextMeshPro.text = newText; // Set the new text
            TargetObject.SetActive(true);
        }
    }
}