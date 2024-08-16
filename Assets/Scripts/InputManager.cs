using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private Grid grid;

    public Vector3Int GetSelectedGridPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = sceneCamera.ScreenToWorldPoint(mousePos);
        Vector3Int gridPosition = grid.WorldToCell(worldPos);
        return gridPosition;
    }
}