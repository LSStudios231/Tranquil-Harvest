using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemData
{
    public int itemId;
    public int count;
}

public static class GameDataManager
{
    private static string inventoryKey = "PlayerInventory";
    private static string goldKey = "PlayerGold";

    public static void SaveInventory(List<Item> inventoryItems)
    {
        List<ItemData> data = new List<ItemData>();
        foreach (var item in inventoryItems)
        {
            data.Add(new ItemData { itemId = item.itemId, count = item.count });
        }

        string json = JsonUtility.ToJson(new Serialization<ItemData>(data));
        PlayerPrefs.SetString(inventoryKey, json);
        PlayerPrefs.Save();
    }

    public static List<Item> LoadInventory()
    {
        string json = PlayerPrefs.GetString(inventoryKey, "{}");
        List<ItemData> data = JsonUtility.FromJson<Serialization<ItemData>>(json).ToList();

        List<Item> inventoryItems = new List<Item>();
        foreach (var itemData in data)
        {
            // Create Item instance and populate with data
            Item item = new Item { itemId = itemData.itemId, count = itemData.count };
            inventoryItems.Add(item);
        }
        return inventoryItems;
    }

    public static void SaveGold(int goldAmount)
    {
        PlayerPrefs.SetInt(goldKey, goldAmount);
        PlayerPrefs.Save();
    }

    public static int LoadGold()
    {
        return PlayerPrefs.GetInt(goldKey, 0);
    }
}

// Helper class for JSON serialization
[System.Serializable]
public class Serialization<T>
{
    public List<T> target;

    public Serialization(List<T> target)
    {
        this.target = target;
    }

    public List<T> ToList()
    {
        return target;
    }
}
