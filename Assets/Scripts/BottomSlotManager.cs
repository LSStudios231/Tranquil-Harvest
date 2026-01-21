using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BottomSlotManager : MonoBehaviour
{
    public GameObject[] uiObjects; // Array of UI objects to be controlled
    public Image[] bottomSlots; // Array of bottom slots images

    private GameObject currentUIObject; // Reference to the currently active UI object

    private void Start()
    {
        // Hide all UI objects initially
        HideAllUIObjects();

        // Assign click listeners to bottom slots images
        AssignClickListeners();
    }

    private void HideAllUIObjects()
    {
        foreach (GameObject obj in uiObjects)
        {
            obj.SetActive(false);
        }
    }

    private void AssignClickListeners()
    {
        for (int i = 0; i < bottomSlots.Length; i++)
        {
            int index = i; // Capture the current index in the loop

            // Add an EventTrigger component if it doesn't already have one
            EventTrigger trigger = bottomSlots[i].gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = bottomSlots[i].gameObject.AddComponent<EventTrigger>();
            }

            // Create a new entry for the PointerClick event
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => OnBottomSlotClicked(index));

            // Add the entry to the EventTrigger component
            trigger.triggers.Add(entry);
        }
    }

    private void OnBottomSlotClicked(int slotIndex)
    {
        // Hide the current UI object if one is active
        if (currentUIObject != null)
        {
            currentUIObject.SetActive(false);
        }

        // Show the UI object corresponding to the selected bottom slot
        if (slotIndex < uiObjects.Length)
        {
            currentUIObject = uiObjects[slotIndex];
            currentUIObject.SetActive(true);
        }
    }
}
