using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class AllowedTiles : MonoBehaviour  {
    [SerializeField] TileBase[] allowedTiles = null;
    [SerializeField] TileBase[] SlowTiles = null;

    public bool Contain(TileBase tile) {
        return allowedTiles.Contains(tile);
    }
    public bool ContainSlow(TileBase tile)
    {
        return SlowTiles.Contains(tile);
    }

    public TileBase[] Get() { return allowedTiles;  }
    public TileBase[] getSlow() { return SlowTiles; }
}
