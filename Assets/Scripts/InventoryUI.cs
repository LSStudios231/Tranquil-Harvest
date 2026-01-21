using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform inventorySlotsParent; // Parent object of the inventory slots
    public Image[] inventorySlotImages; // Images representing inventory slots
    public TMP_Text[] inventorySlotTexts; // TextMeshPro Text components representing item counts in inventory slots

    public Transform bottomBarSlotsParent; // Parent object of the bottom bar slots
    public Image[] bottomBarSlotImages; // Images representing bottom bar slots
    public TMP_Text[] bottomBarSlotTexts; // TextMeshPro Text components representing item counts in bottom bar slots

    public Image shopImage; // Image in the shop panel where items can be sold
    public GameObject inventoryUI; // Reference to the inventory UI GameObject
    public GameObject shopUI; // Reference to the shop UI GameObject
    public TMP_Text goldText; // TextMeshPro Text component to display the player's gold

    private List<Item> items = new List<Item>(); // List of items in the inventory
    private int selectedBottomBarItemIndex = -1; // Index of the selected item in the bottom bar
    private int goldAmount = 0; // The player's current gold amount

    private Image draggedItemImage; // Image component of the currently dragged item
    private TMP_Text draggedItemText; // Text component of the currently dragged item
    private Vector2 touchOffset; // Offset between touch position and item center
    private Transform originalParent; // Original parent of the dragged item

    private HashSet<int> sellableItemIds = new HashSet<int> { 10, 11, 12, 13, 14 }; // Set of sellable item IDs

    private void Start()
    {
        UpdateGoldUI();
        UpdateBottomBar();
    }

    // Method to toggle the visibility of the inventory UI
    public void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        if (inventoryUI.activeSelf)
        {
            UpdateBottomBar(); // Update the bottom bar when the inventory UI is opened
        }
    }

    // Method to toggle the visibility of the shop UI
    public void ToggleShopUI()
    {
        shopUI.SetActive(!shopUI.activeSelf);
        if (shopUI.activeSelf)
        {
            UpdateBottomBar(); // Update the bottom bar when the shop UI is opened
        }
    }

    // Call this method whenever the inventory is updated or when the game starts
    public void UpdateBottomBar()
    {
        int itemCount = Mathf.Min(items.Count, bottomBarSlotImages.Length);

        // Clear all bottom bar slots
        for (int i = 0; i < bottomBarSlotImages.Length; i++)
        {
            if (i < itemCount && items[i].count > 0)
            {
                bottomBarSlotImages[i].sprite = items[i].itemImage;
                bottomBarSlotTexts[i].text = items[i].count.ToString();
            }
            else
            {
                bottomBarSlotImages[i].sprite = null; // Clear the sprite
                bottomBarSlotTexts[i].text = ""; // Clear the text
            }

            // Remove any existing EventTrigger components to prevent duplicate listeners
            EventTrigger trigger = bottomBarSlotImages[i].gameObject.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                Destroy(trigger);
            }

            // Add a new EventTrigger component to handle slot clicks
            trigger = bottomBarSlotImages[i].gameObject.AddComponent<EventTrigger>();

            // Create a new entry for the PointerClick event
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int index = i; // Capture the current index in the loop
            entry.callback.AddListener((eventData) => SelectBottomBarItem(index));

            // Add the entry to the EventTrigger component
            trigger.triggers.Add(entry);
        }
    }

    // Method to add item to the inventory
    public void AddItemToInventory(Item item, int amount = 1)
    {
        bool itemFound = false;

        // Check if the item already exists in the inventory
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemId == item.itemId)
            {
                items[i].count += amount;
                itemFound = true;
                break;
            }
        }

        // If the item does not exist, add it to the inventory
        if (!itemFound)
        {
            item.count = amount;
            items.Add(item);
        }

        // Update inventory UI slots
        UpdateInventorySlots();
        UpdateBottomBar(); // Update the bottom bar
    }
    public void RemoveItemFromInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemId == item.itemId)
            {
                items[i].count--;
                if (items[i].count <= 0)
                {
                    items.RemoveAt(i);
                }
                break;
            }
        }

        // Update the UI to reflect the removal
        UpdateInventorySlots();
        UpdateBottomBar();
    }


    // Method to add item to the bottom navigation bar
    public void AddItemToBottomBar(Item item, int amount = 1)
    {
        bool itemFound = false;

        // Check if the item already exists in the bottom bar
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemId == item.itemId)
            {
                items[i].count += amount;
                itemFound = true;
                break;
            }
        }

        // If the item does not exist, add it to the inventory
        if (!itemFound)
        {
            item.count = amount;
            items.Add(item);
        }

        // Update bottom bar UI slots
        UpdateBottomBar();
    }

    // Helper method to update the inventory slots UI
    public void UpdateInventorySlots()
    {
        for (int i = 0; i < inventorySlotImages.Length; i++)
        {
            if (i < items.Count && items[i].count > 0)
            {
                inventorySlotImages[i].sprite = items[i].itemImage;
                inventorySlotTexts[i].text = items[i].count.ToString();
            }
            else
            {
                inventorySlotImages[i].sprite = null; // Clear the sprite
                inventorySlotTexts[i].text = ""; // Clear the text
            }
        }
    }

    public void SelectBottomBarItem(int index)
    {
        if (index >= 0 && index < items.Count && items[index].count > 0)
        {
            selectedBottomBarItemIndex = index; // Corrected variable name
            Debug.Log($"Selected bottom bar item index: {selectedBottomBarItemIndex}");

            // Highlight or visually indicate the selected item, if needed
        }
        else
        {
            Debug.LogError($"Invalid index or item count is zero: {index}");
        }
    }

    public Item GetSelectedBottomBarItem()
    {
        if (selectedBottomBarItemIndex >= 0 && selectedBottomBarItemIndex < items.Count && items[selectedBottomBarItemIndex].count > 0)
        {
            Item selectedItem = items[selectedBottomBarItemIndex];
            Debug.Log($"Selected bottom bar item: {selectedItem.itemName} with ID {selectedItem.itemId}");
            return selectedItem;
        }
        Debug.Log("No item selected in the bottom bar or invalid index");
        return null;
    }

    // Method to update the gold display UI
    private void UpdateGoldUI()
    {
        goldText.text = goldAmount.ToString();
    }

    // Method to add gold
    public void AddGold(int amount)
    {
        goldAmount += amount;
        UpdateGoldUI();
    }

    // Method to subtract gold
    public bool SubtractGold(int amount)
    {
        if (goldAmount >= amount)
        {
            goldAmount -= amount;
            UpdateGoldUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false;
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                // Check if touch hits any item in the inventory or bottom bar
                if (inventoryUI.activeSelf || shopUI.activeSelf)
                {
                    if (TryBeginDrag(touchPosition, inventorySlotsParent, inventorySlotImages, inventorySlotTexts) ||
                        TryBeginDrag(touchPosition, bottomBarSlotsParent, bottomBarSlotImages, bottomBarSlotTexts))
                    {
                        // Item is touched, start dragging
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && draggedItemImage != null)
            {
                // Drag the item with touch
                draggedItemImage.transform.position = touchPosition + touchOffset;
            }
            else if (touch.phase == TouchPhase.Ended && draggedItemImage != null)
            {
                // Drop the item into a slot
                if (!TryDropItem(touchPosition, inventorySlotsParent, inventorySlotImages, inventorySlotTexts) &&
                    !TryDropItem(touchPosition, bottomBarSlotsParent, bottomBarSlotImages, bottomBarSlotTexts))
                {
                    TryDropItemOnShop(touchPosition);
                }

                // Reset dragged item to its original position and parent
                draggedItemImage.transform.SetParent(originalParent);
                draggedItemImage.transform.localPosition = Vector2.zero;
                draggedItemImage = null;
                draggedItemText = null;
            }
        }
    }

    private bool TryBeginDrag(Vector2 touchPosition, Transform slotsParent, Image[] slotImages, TMP_Text[] slotTexts)
    {
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            RectTransform slot = slotsParent.GetChild(i) as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(slot, touchPosition))
            {
                draggedItemImage = slotImages[i];
                draggedItemText = slotTexts[i];
                touchOffset = (Vector2)draggedItemImage.transform.position - touchPosition;

                originalParent = draggedItemImage.transform.parent;
                draggedItemImage.transform.SetParent(transform); // Change parent to keep it on top
                draggedItemImage.transform.SetAsLastSibling();
                return true;
            }
        }
        return false;
    }

    private bool TryDropItem(Vector2 touchPosition, Transform slotsParent, Image[] slotImages, TMP_Text[] slotTexts)
    {
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            RectTransform slot = slotsParent.GetChild(i) as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(slot, touchPosition))
            {
                SwapItems(draggedItemImage, slotImages[i], draggedItemText, slotTexts[i]);
                return true;
            }
        }
        return false;
    }

    private void SwapItems(Image img1, Image img2, TMP_Text txt1, TMP_Text txt2)
    {
        Sprite tempSprite = img1.sprite;
        string tempText = txt1.text;

        img1.sprite = img2.sprite;
        txt1.text = txt2.text;

        img2.sprite = tempSprite;
        txt2.text = tempText;
    }

    private bool TryDropItemOnShop(Vector2 touchPosition)
    {
        RectTransform shopRect = shopImage.GetComponent<RectTransform>();
        if (RectTransformUtility.RectangleContainsScreenPoint(shopRect, touchPosition))
        {
            SellItem(draggedItemImage);
            return true;
        }
        return false;
    }

    private void SellItem(Image itemImage)
    {
        // Find the item in the inventory or bottom bar
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemImage == itemImage.sprite)
            {
                // Check if the item's ID is in the sellable list
                if (sellableItemIds.Contains(items[i].itemId))
                {
                    // Calculate total value based on item count
                    int itemValue = items[i].value * items[i].count;
                    AddGold(itemValue);

                    // Remove the item from the inventory
                    items.RemoveAt(i);
                    UpdateInventorySlots();
                    UpdateBottomBar();
                    return;
                }
                else
                {
                    Debug.Log("This item cannot be sold.");
                }
            }
        }
    }
}
