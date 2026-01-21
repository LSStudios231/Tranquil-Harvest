using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeasonManager : MonoBehaviour
{
    public enum Season { Summer, Autumn, Winter, Spring }
    public Season currentSeason;

    public float seasonDuration = 1200f; // Time in seconds for each season
    private float seasonTimer;

    // Tile assets for each season
    public TileBase summerTile;
    public TileBase autumnTile;
    public TileBase winterTile;
    public TileBase springTile;

    // Sprites for tree in each season
    public Sprite summerTreeSprite;
    public Sprite autumnTreeSprite;
    public Sprite winterTreeSprite;
    public Sprite springTreeSprite;

    // Reference to the tilemap
    public Tilemap tilemap;

    // A list of all the tree objects in the scene
    private List<GameObject> treeObjects;

    // Start is called before the first frame update
    void Start()
    {
        currentSeason = Season.Summer;
        seasonTimer = seasonDuration;
        treeObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tree"));
    }

    // Update is called once per frame
    void Update()
    {
        seasonTimer -= Time.deltaTime;
        if (seasonTimer <= 0)
        {
            ChangeSeason();
            seasonTimer = seasonDuration;
        }
    }

    void ChangeSeason()
    {
        currentSeason = (Season)(((int)currentSeason + 1) % 4);
        Debug.Log("Season changed to: " + currentSeason);
        UpdateTilesAndTrees();
    }

    void UpdateTilesAndTrees()
    {
        TileBase newTile;
        Sprite newTreeSprite;

        switch (currentSeason)
        {
            case Season.Summer:
                newTile = summerTile;
                newTreeSprite = summerTreeSprite;
                break;
            case Season.Autumn:
                newTile = autumnTile;
                newTreeSprite = autumnTreeSprite;
                break;
            case Season.Winter:
                newTile = winterTile;
                newTreeSprite = winterTreeSprite;
                break;
            case Season.Spring:
                newTile = springTile;
                newTreeSprite = springTreeSprite;
                break;
            default:
                return;
        }

        // Update tiles
        UpdateTiles(newTile);

        // Update trees
        foreach (GameObject tree in treeObjects)
        {
            SpriteRenderer spriteRenderer = tree.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = newTreeSprite;
            }
        }
    }

    void UpdateTiles(TileBase newTile)
    {
        // Iterate through all the positions in the tilemap and update the tiles
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                tilemap.SetTile(position, newTile);
            }
        }
    }
}
