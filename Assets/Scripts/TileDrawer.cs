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
    public Color color;

    private float animationPhase;
    private float animationFrequency;

    // Start is called before the first frame update
    void Start()
    {
        DrawTile();
        animationPhase = Random.Range(0, Mathf.PI);
        animationFrequency = Random.Range(0.9f, 1.1f);
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = 0.8f + Mathf.Sin(
            animationFrequency * Time.time + animationPhase) / 6.0f;
        tilemap.SetColor(position, color.WithAlpha(alpha));
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

    public void BreakingTile()
    {
        tilemap.SetTile(position, null);
        Destroy(gameObject);
    }
}
