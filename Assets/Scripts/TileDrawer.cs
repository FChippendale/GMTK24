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

    public FactoryBehaviour.TraversalType traversalType;

    public bool active = true;

    Dictionary<FactoryBehaviour.TraversalType, Color> mapping = new Dictionary<FactoryBehaviour.TraversalType, Color>{
        {FactoryBehaviour.TraversalType.constant_integer_amount, new Color(0.20392f, 0.34902f, 0.58431f, 1.0f)}, // 345995
        {FactoryBehaviour.TraversalType.sum_of_any_adjacent, new Color(0.89412f, 0.0f, 0.4f, 1.0f)},    // E40066
        {FactoryBehaviour.TraversalType.largest_adjacent, new Color(0.01176f, 080784f, 0.64314f, 1.0f)} // 03CEA4
        
        // FB4D3D
    };

    // Start is called before the first frame update
    void Start()
    {   
        drawTile();
    }

    // Update is called once per frame
    void Update()
    {
        toggleTile();
    }

    private void toggleTile()
    {
        // active = !active;
        // don't toggle a tile if there isn't one
        if (!tilemap.HasTile(position))
        {
            return;
        }
        Color tempColor = mapping[traversalType];
        tempColor.a = (active ? 1.0f : 0.1f);

        tilemap.SetColor(position, tempColor);
    } 

    private void drawTile()
    {
        // don't draw a tile if there already is one
        if (tilemap.HasTile(position))
        {
            return;
        }

        tilemap.SetTile(position, tileAsset);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, mapping[traversalType]);
    }
}
