using System;
using System.Collections;
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

    // simple utility to move an object to the on screen position of the currently returned grid position
    private void Update()
    {
        // get mouse grid position and convert back to screen pos
        Vector3Int gridPosition = inputManager.GetSelectedGridPosition();
        drawTile(gridPosition);

        Vector3 indicatorPosition = grid.CellToWorld(gridPosition);

        // move sprite back a bit so it renders on the camera
        indicatorPosition.z += 1f;

        // position sprite at calculated position
        cellIndicator.transform.position = indicatorPosition;
    }

    private void drawTile(Vector3Int target)
    {
        // don't draw a tile if there already is one
        if(tilemap.HasTile(target))
        {
            return;
        }

        tilemap.SetTile(target, tileAsset);
    }
}