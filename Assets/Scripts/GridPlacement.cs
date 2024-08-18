using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridPlacement : MonoBehaviour
{
    private readonly TileGrid grid = new();

    public List<(int, int)> GetPossiblePlacements()
    {
        return grid.GetPossiblePlacements();
    }

    public bool TryAddToGrid(List<GameObject> to_add, List<(int, int)> positions, bool allow_island)
    {
        return grid.TryAddTile(to_add, positions, allow_island);
    }

    public bool CheckForEncirclements(FactoryBehaviour.TraversalType type)
    {
        HashSet<(int, int)> encircled_tiles = grid.GetTilesEncircledBy(type);
        if (encircled_tiles.Count == 0)
        {
            return false;
        }

        HashSet<(int, int)> tiles_to_destroy = new HashSet<(int, int)>(encircled_tiles);
        tiles_to_destroy = grid.ExpandRingIntoTouching(tiles_to_destroy, type);
        tiles_to_destroy = grid.ExpandByRing(tiles_to_destroy);

        foreach (var (x, y) in tiles_to_destroy)
        {
            if (grid.HasOccupier(x, y))
            {
                GameObject tile = grid.GetOccupier(x, y);
                tile.SendMessage("BreakingTile");
                grid.RemoveTile(x, y);
            }
        }
        return true;
    }

    public (int, int) GetCenterTile()
    {
        return grid.GetCenterTile();
    }

    public void Update()
    {
        // Get enclosed space
    }
}
