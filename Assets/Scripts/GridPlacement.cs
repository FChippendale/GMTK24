using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridPlacement : MonoBehaviour
{
    [SerializeField]
    private TriggerSFX triggerSFX;

    public readonly TileGrid grid = new();

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

        bool encircles_tiles = false;
        HashSet<(int, int)> tiles_to_destroy = new HashSet<(int, int)>(encircled_tiles);
        int encircled_tile_count = tiles_to_destroy.Count;

        foreach (var (x, y) in tiles_to_destroy)
        {
            if (grid.HasOccupier(x, y))
            {
                encircles_tiles = true;
                break;
            }
        }

        if (!encircles_tiles)
        {
            return false;
        }

        tiles_to_destroy = grid.ExpandRingIntoTouching(tiles_to_destroy, type);
        tiles_to_destroy = grid.ExpandByRing(tiles_to_destroy);

        int tiles_to_break = 0;
        triggerSFX.PlaySound(TriggerSFX.SoundType.break_block);
        foreach (var (x, y) in tiles_to_destroy)
        {
            if (grid.HasBreakableOccupier(x, y))
            {
                GameObject tile = grid.GetOccupier(x, y);
                tile.SendMessage("BreakingTile");
                grid.RemoveTile(x, y);
                tiles_to_break += 1;
            }
        }

        gameObject.SendMessage("BreakingTiles", (tiles_to_break, encircled_tile_count));
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
