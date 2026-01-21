using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public GameObject panel; // Reference to the Panel GameObject
    public Button toggleButton; // Reference to the Button

    void Start()
    {
        // Ensure the panel is disabled at the start
        panel.SetActive(false);

        // Add listener to the button to handle the toggle action
        toggleButton.onClick.AddListener(TogglePanel);
    }

    void TogglePanel()
    {
        // Toggle the panel's active state
        panel.SetActive(!panel.activeSelf);
    }
}
