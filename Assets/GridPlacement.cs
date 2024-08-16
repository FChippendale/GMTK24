using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridPlacement : MonoBehaviour
{
    public TileGrid grid;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TryAddToGrid(GameObject to_add, int x, int y)
    {
        return grid.TryAddTile(to_add, x, y);
    }
}
