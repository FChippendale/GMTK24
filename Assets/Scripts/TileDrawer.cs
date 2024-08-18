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

    private bool IsCurrentlyBeingDestroyed = false;
    public float DestroyFlashFrequency = 20.0f;
    private float DestroyRequestStartTime = 0.0f;
    public int FlashesTillDestroy = 3;
    private float timeToDestroy = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        DrawTile();
        animationPhase = Random.Range(0, Mathf.PI);
        animationFrequency = Random.Range(0.9f, 1.1f);
        timeToDestroy = -Mathf.PI / DestroyFlashFrequency + FlashesTillDestroy * (2.0f * Mathf.PI / DestroyFlashFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCurrentlyBeingDestroyed)
        {
            if (Time.time - DestroyRequestStartTime >= timeToDestroy)
            {
                tilemap.SetTile(position, null);
                Destroy(gameObject);
            }
            float alpha = 0.5f + 0.5f * Mathf.Cos(DestroyFlashFrequency * (Time.time - DestroyRequestStartTime));
            tilemap.SetColor(position, color.WithAlpha(alpha));

            // Pause any other animations
            return;
        }

        if (traversalType != FactoryBehaviour.TraversalType.unbreakable_starting_tile)
        {
            float alpha = 0.8f + Mathf.Sin(
                animationFrequency * Time.time + animationPhase) / 6.0f;
            tilemap.SetColor(position, color.WithAlpha(alpha));
        }
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
        IsCurrentlyBeingDestroyed = true;
        DestroyRequestStartTime = Time.time;
    }
}
