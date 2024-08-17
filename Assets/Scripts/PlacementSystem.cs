using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    private GameObject cellIndicator;

    [SerializeField]
    private GameObject factoryToPlace;

    private GridPlacement gridPlacement;

    [SerializeField]
    private Camera sceneCamera;

    private bool TryAddFactoryAtPosition(Vector3Int gridPosition, bool allow_island = false)
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

        // pass factory behaviour on creation to allow redrawing from TileDrawer
        drawer.traversalType = toPlace.GetComponent<FactoryBehaviour>().traversalType;
        drawer.color = toPlace.GetComponent<FactoryBehaviour>().getColor();
        drawer.position = gridPosition;

        FactoryBehaviour behaviour = toPlace.GetComponent<FactoryBehaviour>();
        behaviour.viewportPosition = sceneCamera.WorldToViewportPoint(grid.CellToWorld(gridPosition));

        gameObject.SendMessage("FactoryAdded", gridPosition);
        assignNextFactoryType();

        return true;
    }

    private void Start()
    {
        gridPlacement = GetComponent<GridPlacement>();
        TryAddFactoryAtPosition(new Vector3Int(0, 0, 0), true);
    }

    void assignNextFactoryType()
    {
        List<FactoryBehaviour.TraversalType> traversalTypes = new List<FactoryBehaviour.TraversalType>{
            FactoryBehaviour.TraversalType.constant_integer_amount,
            FactoryBehaviour.TraversalType.largest_adjacent,
            FactoryBehaviour.TraversalType.sum_of_any_adjacent,
        };

        FactoryBehaviour.TraversalType type = traversalTypes[UnityEngine.Random.Range(0, traversalTypes.Count)];
        factoryToPlace.GetComponent<FactoryBehaviour>().traversalType = type;
    }

    public void PlacementDeadlineTimerTick()
    {
        List<(int, int)> possiblePlacements = gridPlacement.GetPossiblePlacements();
        var (x, y) = possiblePlacements[UnityEngine.Random.Range(0, possiblePlacements.Count)];
        gridPlacement.TryAddToGrid(factoryToPlace, x, y, true);
        assignNextFactoryType();
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

        // move sprite back a bit so it renders on the camera
        indicatorPosition.z += 1f;

        // position sprite at calculated position
        cellIndicator.transform.position = indicatorPosition;

        Color colour;

        if (gridPosition.type == PositionType.valid)
        {
            colour = factoryToPlace.GetComponent<FactoryBehaviour>().getColor();

            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                if (!TryAddFactoryAtPosition(gridPosition.position, false))
                {
                    colour = Color.red.WithAlpha(0.5f);
                }
            }
        }
        else
        {
            colour = (Input.GetMouseButtonDown((int)MouseButton.Left) ? Color.red : Color.gray).WithAlpha(0.5f);
        }

        cellIndicator.GetComponent<SpriteRenderer>().material.color = colour;
    }
}
