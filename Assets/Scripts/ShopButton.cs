using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public InventoryUI inventoryUI; // Reference to the InventoryUI script
    public int itemID; // ID of the item to be added
    public int itemCount = 1; // Number of items to be added
    public int itemPrice; // Price of the item in gold
    public Sprite itemImage; // Image of the item to be added

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (inventoryUI.SubtractGold(itemPrice))
        {
            Item newItem = new Item
            {
                itemId = itemID,
                itemName = "NewItem", // Replace with actual item name
                itemImage = itemImage,
                count = itemCount,
                value = itemPrice // Replace with actual item value
            };
            inventoryUI.AddItemToBottomBar(newItem, itemCount);
        }
    }
}
