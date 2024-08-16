using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDrawer : MonoBehaviour
{
    [SerializeField]
    private TileBase tileAsset;

    public Tilemap tilemap;

    public Vector3Int position;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        drawTile(position);
    }

    private void drawTile(Vector3Int target)
    {
        // don't draw a tile if there already is one
        if (tilemap.HasTile(target))
        {
            return;
        }

        tilemap.SetTile(target, tileAsset);
    }
}
