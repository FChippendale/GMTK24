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

    private void Start()
    {
        gridPlacement = GetComponent<GridPlacement>();
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
            var (center_x, center_y) = gridPlacement.GetCenterTile();
            GameObject toPlace = Instantiate(factoryToPlace);
            toPlace.GetComponent<TileDrawer>().tilemap = tilemap;
            toPlace.GetComponent<TileDrawer>().position = gridPosition;

            GetComponent<GridPlacement>().TryAddToGrid(toPlace, center_x - gridPosition.x, center_y - gridPosition.y, true);
        }
    }
}
