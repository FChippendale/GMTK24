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
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        DrawTile();
    }

    // Update is called once per frame
    void Update()
    {
        ToggleTile();
    }

    private void ToggleTile()
    {
        // active = !active;
        // don't toggle a tile if there isn't one
        if (!tilemap.HasTile(position))
        {
            return;
        }
        Color tempColor = color;
        tempColor.a = active ? 1.0f : 0.1f;

        tilemap.SetColor(position, tempColor);
    }

    private void DrawTile()
    {
        // don't draw a tile if there already is one
        if (tilemap.HasTile(position))
        {
            return;
        }

        tilemap.SetTile(position, tileAsset);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
