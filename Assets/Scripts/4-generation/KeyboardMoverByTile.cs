using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    int limit = 10;
    int semiLimit = 3;
    int counter = 0;
    int semiCounter = 0;
    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }
    void Update()  {
        if (0 == counter)
        {
            if (!allowedTiles.ContainSlow(tilemap.GetTile(tilemap.WorldToCell(transform.position)))) { 
                Vector3 newPosition = NewPosition();
                TileBase tileOnNewPosition = TileOnPosition(newPosition);
                if (allowedTiles.Contain(tileOnNewPosition))
                {
                    transform.position = newPosition;
                }
                else
                {
                    Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
                }
            }
            else
            {
                if (semiCounter == 0)
                {
                    Vector3 newPosition = NewPosition();
                    TileBase tileOnNewPosition = TileOnPosition(newPosition);
                    if (allowedTiles.Contain(tileOnNewPosition))
                    {
                        transform.position = newPosition;
                    }
                    else
                    {
                        Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
                    }
                }
                semiCounter++;
                if (semiCounter == semiLimit) semiCounter = 0;
            }
        }
        counter++;
        if (counter == limit) counter = 0;
    }
}
