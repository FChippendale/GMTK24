using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private GameObject mouseIndicator;

    private void Update()
    {
        Vector3Int gridPosition = inputManager.GetSelectedGridPosition();
        Vector3 indicatorPosition = grid.CellToWorld(gridPosition);
        indicatorPosition.z += 1f;
        mouseIndicator.transform.position = indicatorPosition;
    }
}