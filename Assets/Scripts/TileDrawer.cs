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

    private FactoryBehaviour.TraversalType traversalType;

    Dictionary<FactoryBehaviour.TraversalType, Color> mapping = new Dictionary<FactoryBehaviour.TraversalType, Color>{
        {FactoryBehaviour.TraversalType.constant_integer_amount, new Color(0.20392f, 0.34902f, 0.58431f, 1.0f)},
        {FactoryBehaviour.TraversalType.sum_of_any_adjacent, new Color(0.89412f, 0.0f, 0.4f, 1.0f)}
        // 03CEA4
        // FB4D3D
    };

    // Start is called before the first frame update
    void Start()
    {   
        traversalType = GetComponent<FactoryBehaviour>().traversalType;
        drawTile(position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void drawTile(Vector3Int target)
    {
        // don't draw a tile if there already is one
        if (tilemap.HasTile(target))
        {
            return;
        }

        tilemap.SetTile(target, tileAsset);
        tilemap.SetTileFlags(target, TileFlags.None);
        tilemap.SetColor(target, mapping[traversalType]);
        
        // tilemap.SetColor(target, mapping[FactoryBehaviour.TraversalType.constant_integer_amount]);
    }
}
