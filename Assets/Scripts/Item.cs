using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "Item"; // Name of the item to be displayed in the inventory
    public int itemId = 0; // Unique ID of the item
    public Sprite itemImage; // Image of the item to be displayed in the inventory UI
    public int count = 1; // Number of items in the stack
    public int value = 10; // Value of the item in gold
    public GameObject plantPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetInteractableItem(this); // Set the item that the player can interact with
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetInteractableItem(null); // Reset the interactable item when the player moves away
            }
        }
    }
}
