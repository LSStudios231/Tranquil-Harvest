using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Button pickupButton; // Reference to the pickup button
    public InventoryUI inventoryUI; // Reference to the inventory UI script
    private Item interactableItem; // Item that the player can interact with
    public GameObject inventoryUIObject;

    private void Start()
    {
        pickupButton.onClick.AddListener(PickupItem); // Add a listener to the pickup button
        inventoryUIObject.SetActive(false);
    }

    public void SetInteractableItem(Item item)
    {
        interactableItem = item;
    }

    private void PickupItem()
    {
        if (interactableItem != null)
        {
            inventoryUI.AddItemToInventory(interactableItem); // Add the item to the inventory UI
            interactableItem.gameObject.SetActive(false); // Deactivate the item instead of destroying it
            interactableItem = null; // Reset the interactable item
        }
    }



}
