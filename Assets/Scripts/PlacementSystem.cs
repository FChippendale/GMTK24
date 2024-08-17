using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase tileAsset;
    [SerializeField]
    private CellIndicator cellIndicator;

    [SerializeField]
    private GameObject factoryToPlace;

    private GridPlacement gridPlacement;

    [SerializeField]
    private Camera sceneCamera;

    public GameObject nextPlacementHint;


    private bool TryAddTileAtGridPosition(Vector3Int gridPosition, bool allow_island = false)
    {
        GameObject toPlace = Instantiate(factoryToPlace, grid.CellToWorld(gridPosition), Quaternion.identity);

        var (center_x, center_y) = gridPlacement.GetCenterTile();
        if (!gridPlacement.TryAddToGrid(toPlace, center_x - gridPosition.x, center_y - gridPosition.y, allow_island))
        {
            // Failed to place tile
            Destroy(toPlace);
            return false;
        }

        TileDrawer drawer = toPlace.GetComponent<TileDrawer>();
        drawer.tilemap = tilemap;

        FactoryBehaviour factoryBehaviourToPlace = toPlace.GetComponent<FactoryBehaviour>();

        // pass factory behaviour on creation to allow redrawing from TileDrawer
        drawer.traversalType = factoryBehaviourToPlace.traversalType;
        drawer.color = factoryBehaviourToPlace.GetColor();
        drawer.position = gridPosition;

        factoryBehaviourToPlace.viewportPosition = sceneCamera.WorldToViewportPoint(grid.CellToWorld(gridPosition));

        gameObject.SendMessage("FactoryAdded", gridPosition);
        AssignNextFactoryType();

        return true;
    }

    private void Start()
    {
        gridPlacement = GetComponent<GridPlacement>();
        TryAddTileAtGridPosition(new Vector3Int(0, 0, 0), true);
    }

    void AssignNextFactoryType()
    {
        List<FactoryBehaviour.TraversalType> traversalTypes = new List<FactoryBehaviour.TraversalType>{
            FactoryBehaviour.TraversalType.constant_integer_amount,
            FactoryBehaviour.TraversalType.largest_adjacent,
            FactoryBehaviour.TraversalType.sum_of_any_adjacent,
        };

        FactoryBehaviour.TraversalType type = traversalTypes[Random.Range(0, traversalTypes.Count)];
        factoryToPlace.GetComponent<FactoryBehaviour>().traversalType = type;
        nextPlacementHint.GetComponent<Image>().color = factoryToPlace.GetComponent<FactoryBehaviour>().GetColor();
    }

    public void PlacementDeadlineTimerTick()
    {
        List<(int, int)> possiblePlacements = gridPlacement.GetPossiblePlacements();
        var (x, y) = possiblePlacements[Random.Range(0, possiblePlacements.Count)];
        var (center_x, center_y) = gridPlacement.GetCenterTile();
        TryAddTileAtGridPosition(new Vector3Int(center_x - x, center_y - y, 0), true);
    }

    // simple utility to move an object to the on screen position of the currently returned grid position
    private void Update()
    {
        // get mouse grid position and convert back to screen pos
        GridPosition gridPosition = inputManager.GetSelectedGridPosition();

        if (gridPosition.type == PositionType.outside_grid)
        {
            // Cursor is not in the grid, there's nothing to do.
            return;
        }

        // Render an indicator on the active tile.
        Vector3 indicatorPosition = grid.CellToWorld(gridPosition.position);

        if (gridPosition.type == PositionType.valid)
        {
            cellIndicator.Reposition(indicatorPosition,
                factoryToPlace.GetComponent<FactoryBehaviour>().GetColor());

            if (Input.GetMouseButtonDown((int)MouseButton.Left) && !TryAddTileAtGridPosition(gridPosition.position, false))
            {
                cellIndicator.StartInvalidAnimation();
            }
        }
        else
        {
            cellIndicator.Reposition(indicatorPosition,
                /* Good Samaritan */ new Color32(0x3c, 0x63, 0x82, 0xff));

            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                cellIndicator.StartInvalidAnimation();
            }
        }
    }
}
