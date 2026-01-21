using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<Item> inventoryItems;
    public InventoryUI inventoryUI; // Reference to your InventoryUI script

    private void Start()
    {
        LoadGameData();
        inventoryUI.UpdateInventorySlots(); // Ensure UI is updated after loading data
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    public void SaveGameData()
    {
        GameDataManager.SaveInventory(inventoryItems);
        GameDataManager.SaveGold(GoldManager.Instance.GetGoldAmount());
    }

    public void LoadGameData()
    {
        inventoryItems = GameDataManager.LoadInventory();
        int playerGold = GameDataManager.LoadGold();
        GoldManager.Instance.SetGoldAmount(playerGold);
        inventoryUI.UpdateInventorySlots();
    }

    // Call this method whenever an item is added or removed
    public void AddItemToInventory(Item item)
    {
        inventoryItems.Add(item);
        inventoryUI.AddItemToInventory(item);
        GameDataManager.SaveInventory(inventoryItems);  // Save immediately if needed
    }

    public void RemoveItemFromInventory(Item item)
    {
        inventoryItems.Remove(item);
        inventoryUI.RemoveItemFromInventory(item);
        GameDataManager.SaveInventory(inventoryItems);  // Save immediately if needed
    }
}
