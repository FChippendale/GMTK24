using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;



public class DeathCircle : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private TileBase tileAsset;


    private HashSet<(int, int)> deathTiles = new HashSet<(int, int)>();

    public bool IsDeathTile((int, int) pos)
    {
        return deathTiles.Contains(pos);
    }

    private void Start()
    {
        // We only care about the range -150, 150, but go a bit overboard
        for (int x = -65; x < 65; x++)
        {
            for (int y = -65; y < 65; y++)
            {
                Vector3 wordPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
                if (wordPosition.magnitude > 45)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    tilemap.SetTile(pos, tileAsset);
                    tilemap.SetTileFlags(pos, TileFlags.None);
                    tilemap.SetColor(pos, Color.white.WithAlpha(0.2f));
                    deathTiles.Add((x, y));
                }
            }
        }
    }
}
