using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the player's movement speed as needed
    public float jumpForce = 5f; // Force applied when the player jumps
    public Button leftButton;
    public Button rightButton;
    public Button upButton;
    public Button downButton;
    public Button farmButton; // Reference to the farm button
    public Animator animator; // Reference to the Animator component
    public Tilemap groundTilemap; // Reference to the ground tilemap
    public TileBase farmTile; // Tile to be placed when the farm button is clicked
    public List<GameObject> plantPrefabs; // List of plant prefabs to be instantiated
    public List<Item> Seeds; // List of seed items
    public LayerMask groundLayer; // LayerMask to specify what is considered ground
    public LayerMask treeLayer; // LayerMask to specify what is considered a tree

    public Image leftArrowImage; // Image for the left arrow
    public Image rightArrowImage; // Image for the right arrow
    public Image upArrowImage; // Image for the up arrow
    public Image downArrowImage; // Image for the down arrow

    private Rigidbody2D rb;
    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private bool isUpPressed = false;
    private bool isDownPressed = false;
    private bool isGrounded = false;

    private InventoryUI inventoryUI; // Reference to the InventoryUI script

    public float maxDistance = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Assign button click listeners using EventTrigger component
        AddEventTrigger(leftButton, EventTriggerType.PointerDown, () => { isLeftPressed = true; leftArrowImage.gameObject.SetActive(true); });
        AddEventTrigger(rightButton, EventTriggerType.PointerDown, () => { isRightPressed = true; rightArrowImage.gameObject.SetActive(true); });
        AddEventTrigger(upButton, EventTriggerType.PointerDown, () => { isUpPressed = true; upArrowImage.gameObject.SetActive(true); });
        AddEventTrigger(downButton, EventTriggerType.PointerDown, () => { isDownPressed = true; downArrowImage.gameObject.SetActive(true); });
        AddEventTrigger(farmButton, EventTriggerType.PointerDown, OnFarmButtonClick); // Add pointer down event for the farm button

        // Add pointer up events for each button
        AddEventTrigger(leftButton, EventTriggerType.PointerUp, () => { isLeftPressed = false; leftArrowImage.gameObject.SetActive(false); });
        AddEventTrigger(rightButton, EventTriggerType.PointerUp, () => { isRightPressed = false; rightArrowImage.gameObject.SetActive(false); });
        AddEventTrigger(upButton, EventTriggerType.PointerUp, () => { isUpPressed = false; upArrowImage.gameObject.SetActive(false); });
        AddEventTrigger(downButton, EventTriggerType.PointerUp, () => { isDownPressed = false; downArrowImage.gameObject.SetActive(false); });

        // Find the InventoryUI script in the scene
        inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI not found in the scene!");
        }

        // Ensure arrow images are initially inactive
        leftArrowImage.gameObject.SetActive(false);
        rightArrowImage.gameObject.SetActive(false);
        upArrowImage.gameObject.SetActive(false);
        downArrowImage.gameObject.SetActive(false);
    }

    private void AddEventTrigger(Button button, EventTriggerType eventType, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventType
        };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }

    private void Update()
    {
        // Move the player based on the button states
        Vector2 movement = Vector2.zero;

        if (isLeftPressed)
        {
            movement += Vector2.left;
            PlayAnimation("MoveLeft");
        }
        if (isRightPressed)
        {
            movement += Vector2.right;
            PlayAnimation("MoveRight");
        }
        if (isUpPressed)
        {
            movement += Vector2.up;
            PlayAnimation("MoveUp");
        }
        if (isDownPressed)
        {
            movement += Vector2.down;
            PlayAnimation("MoveDown");
        }

        movement = movement.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Jump when the spacebar is pressed and the player is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Check for player input to chop down a tree
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChopTree();
        }
    }

    private void PlayAnimation(string animationTrigger)
    {
        // Trigger the animation
        animator.SetTrigger(animationTrigger);
    }

    private void OnFarmButtonClick()
    {
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI reference is null!");
            return;
        }

        Item selectedItem = inventoryUI.GetSelectedBottomBarItem();
        if (selectedItem != null)
        {
            Debug.Log("Selected bottom bar item: " + selectedItem.itemName + " with ID " + selectedItem.itemId);

            // Check if the selected item is the axe (ID 8)
            if (selectedItem.itemId == 8)
            {
                Debug.Log("Destroying tree because axe is selected.");
                DestroyTree();
            }
            else if (selectedItem.itemId == 7) // Check if the selected item is the shovel (ID 7)
            {
                Debug.Log("Shovel selected. Proceeding to change tile.");
                Vector3Int playerCellPosition = groundTilemap.WorldToCell(transform.position);
                groundTilemap.SetTile(playerCellPosition, farmTile);
            }
            else
            {
                Debug.Log("Planting seed because another item is selected.");
                PlantSeed(selectedItem);
            }
        }
        else
        {
            Debug.Log("No item selected.");
        }
    }

    void PlantSeed(Item selectedItem)
    {
        if (selectedItem.count <= 0)
        {
            Debug.Log("No seeds left to plant.");
            return;
        }

        // Find the seed associated with the given item ID
        Item seed = Seeds.Find(s => s.itemId == selectedItem.itemId);

        // Check if the seed was found
        if (seed != null)
        {
            // Access the plant prefab associated with the seed
            GameObject plantPrefab = seed.plantPrefab;

            // Check if the prefab was found
            if (plantPrefab != null)
            {
                // Get the cell size of the ground tilemap
                Vector3 cellSize = groundTilemap.cellSize;

                // Calculate the position with the offset
                Vector3 spawnPosition = groundTilemap.GetCellCenterWorld(groundTilemap.WorldToCell(transform.position));

                // Set the z position to -1
                spawnPosition.z = -1;

                // Instantiate the prefab at the desired position
                Instantiate(plantPrefab, spawnPosition, Quaternion.identity);

                // Decrease the seed count
                selectedItem.count--;

                // Check if the seed count has reached zero
                if (selectedItem.count <= 0)
                {
                    // Remove the seed from the inventory
                    Seeds.Remove(selectedItem);
                }

                // Update the inventory UI
                inventoryUI.UpdateInventorySlots();
                inventoryUI.UpdateBottomBar();
            }
            else
            {
                Debug.LogError("Plant prefab not found for seed itemId: " + selectedItem.itemId);
            }
        }
        else
        {
            Debug.LogError("Seed not found for itemId: " + selectedItem.itemId);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        // Check for player input to interact with tiles
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithTile();
        }
    }

    private void InteractWithTile()
    {
        // Get the position of the player in world space
        Vector3Int playerCellPosition = groundTilemap.WorldToCell(transform.position);

        // Check the type of tile the player is standing on
        TileBase currentTile = groundTilemap.GetTile(playerCellPosition);

        // Check if the player has the seed selected in their inventory
        bool hasSeedSelected = CheckSeedInInventory();

        // If the player is standing on a specific tile and
        // conditions are met, interact with the tile
    }

    private bool CheckSeedInInventory()
    {
        // Check if the player has a seed selected in their inventory
        // This function can be implemented to check the player's inventory for seeds
        return true; // Placeholder value
    }

    private void ChopTree()
    {
        Vector2 raycastDirection = Vector2.up;
        Debug.Log("Raycast position: " + transform.position);
        Debug.Log("Raycast direction: " + raycastDirection);

        // Cast a ray to check for trees in front of the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, 10f, treeLayer); // Increased raycast distance to 10f

        Debug.Log("Raycast hit: " + hit.collider);

        if (hit.collider != null)
        {
            Tree tree = hit.collider.GetComponent<Tree>();
            if (tree != null)
            {
                Destroy(tree.gameObject);
            }
        }
    }

    private void DestroyTree()
    {
        Vector2 raycastDirection = Vector2.up;
        Debug.Log("Raycast position: " + transform.position);
        Debug.Log("Raycast direction: " + raycastDirection);

        // Cast a ray to check for trees in front of the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, 10f, treeLayer); // Increased raycast distance to 10f

        Debug.Log("Raycast hit: " + hit.collider);

        if (hit.collider != null)
        {
            Tree tree = hit.collider.GetComponent<Tree>();
            if (tree != null)
            {
                Destroy(tree.gameObject);
            }
        }
    }
}
