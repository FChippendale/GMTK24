using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

    private void tryAddFactoryAtPosition(Vector3Int gridPosition, bool allow_island = false)
    {
        GameObject toPlace = Instantiate(factoryToPlace);

        var (center_x, center_y) = gridPlacement.GetCenterTile();
        if (!gridPlacement.TryAddToGrid(toPlace, center_x - gridPosition.x, center_y - gridPosition.y, allow_island))
        {
            // Failed to place tile
            Destroy(toPlace);
            return;
        }

        TileDrawer drawer = toPlace.GetComponent<TileDrawer>();
        drawer.tilemap = tilemap;
        drawer.position = new Vector3Int(gridPosition.x, gridPosition.y, 0);
    }

    private void Start()
    {
        gridPlacement = GetComponent<GridPlacement>();
        tryAddFactoryAtPosition(new Vector3Int(0, 0, 0), true);
    }

    // simple utility to move an object to the on screen position of the currently returned grid position
    private void Update()
    {
        // get mouse grid position and convert back to screen pos
        Vector3Int gridPosition = inputManager.GetSelectedGridPosition();

        Vector3 indicatorPosition = grid.CellToWorld(gridPosition);

        // move sprite back a bit so it renders on the camera
        indicatorPosition.z += 1f;

        // position sprite at calculated position
        cellIndicator.transform.position = indicatorPosition;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            tryAddFactoryAtPosition(gridPosition, false);
        }
    }
}
