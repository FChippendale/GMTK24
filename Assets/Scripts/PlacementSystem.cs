using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private TriggerSFX triggerSFX;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase tileAsset;
    [SerializeField]
    private List<CellIndicator> CellIndicators;

    [SerializeField]
    private GameObject factoryToPlace;

    private bool[,,] tileToPlace;

    private GridPlacement gridPlacement;

    [SerializeField]
    private Camera sceneCamera;

    public GameObject nextPlacementHint;


    private bool TryAddTileAtGridPosition(Vector3Int gridPosition, bool allow_island = false)
    {
        var (center_x, center_y) = gridPlacement.GetCenterTile();

        List<GameObject> gridItems = new List<GameObject>();
        List<(int, int)> gridCoords = new List<(int, int)>();
        List<(int, int)> unityCoords = TileBag.ConvertToUnityCoords(tileToPlace, gridPosition.x, gridPosition.y);
        foreach (var (x, y) in unityCoords)
        {
            gridCoords.Add((x + center_x, y + center_y));
            GameObject toPlace = Instantiate(factoryToPlace, grid.CellToWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
            gridItems.Add(toPlace);
        }

        if (!gridPlacement.TryAddToGrid(gridItems, gridCoords, allow_island))
        {
            // Failed to place tile
            foreach (GameObject obj in gridItems)
            {
                Destroy(obj);
            }
            return false;
        }

        // Finalize each factor state
        for (int i = 0; i < gridItems.Count; i++)
        {
            GameObject obj = gridItems[i];
            var (x, y) = unityCoords[i];


            TileDrawer drawer = obj.GetComponent<TileDrawer>();
            drawer.tilemap = tilemap;

            FactoryBehaviour factoryBehaviourToPlace = obj.GetComponent<FactoryBehaviour>();

            // pass factory behaviour on creation to allow redrawing from TileDrawer
            drawer.traversalType = factoryBehaviourToPlace.traversalType;
            drawer.color = factoryBehaviourToPlace.GetColor();
            drawer.position = new Vector3Int(x, y, 0);

            factoryBehaviourToPlace.viewportPosition = sceneCamera.WorldToViewportPoint(grid.CellToWorld(drawer.position));
        }
        triggerSFX.PlaySound(TriggerSFX.SoundType.placement);

        // Has the user scored any points?
        gridPlacement.CheckForEncirclements(factoryToPlace.GetComponent<FactoryBehaviour>().traversalType);

        gameObject.SendMessage("FactoryAdded", gridPosition);
        AssignNextFactoryType();

        return true;
    }

    private void Start()
    {
        tileToPlace = new bool[4, 4, 4];
        tileToPlace[0, 0, 0] = true;

        gridPlacement = GetComponent<GridPlacement>();
        factoryToPlace.GetComponent<FactoryBehaviour>().traversalType = FactoryBehaviour.TraversalType.unbreakable_starting_tile;
        TryAddTileAtGridPosition(new Vector3Int(0, 0, 0), true);
    }

    void AssignNextFactoryType()
    {
        List<FactoryBehaviour.TraversalType> traversalTypes = new List<FactoryBehaviour.TraversalType>{
            FactoryBehaviour.TraversalType.constant_integer_amount,
            FactoryBehaviour.TraversalType.largest_adjacent,
            FactoryBehaviour.TraversalType.sum_of_any_adjacent,
        };

        FactoryBehaviour.TraversalType type = traversalTypes[Random.Range(0, traversalTypes.Count)];
        factoryToPlace.GetComponent<FactoryBehaviour>().traversalType = type;
        nextPlacementHint.GetComponent<Image>().color = factoryToPlace.GetComponent<FactoryBehaviour>().GetColor();

        tileToPlace = TileBag.GetRandomShape();
    }

    public void PlacementDeadlineTimerTick()
    {
    }


    private void DrawPlacementHint(Vector3Int mouseGridPosition, Color color)
    {
        List<(int, int)> unityCoords = TileBag.ConvertToUnityCoords(tileToPlace, mouseGridPosition.x, mouseGridPosition.y);
        for (int i = 0; i < unityCoords.Count; i++)
        {
            CellIndicator indicator = CellIndicators[i];
            var (x, y) = unityCoords[i];

            Vector3 indicatorPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
            indicator.Reposition(indicatorPosition, color);
        }
    }

    // simple utility to move an object to the on screen position of the currently returned grid position
    private void Update()
    {
        // get mouse grid position and convert back to screen pos
        GridPosition gridPosition = inputManager.GetSelectedGridPosition();

        if (gridPosition.type == PositionType.outside_grid)
        {
            // Cursor is not in the grid, there's nothing to do.
            return;
        }

        if (gridPosition.type == PositionType.valid)
        {
            DrawPlacementHint(gridPosition.position, factoryToPlace.GetComponent<FactoryBehaviour>().GetColor());

            if (Input.GetMouseButtonDown((int)MouseButton.Left) && !TryAddTileAtGridPosition(gridPosition.position, false))
            {
                foreach (CellIndicator indicator in CellIndicators)
                {
                    triggerSFX.PlaySound(TriggerSFX.SoundType.invalid_placement);
                    indicator.StartInvalidAnimation();
                }
            }
        }
        else
        {
            DrawPlacementHint(gridPosition.position, /* Good Samaritan */ new Color32(0x3c, 0x63, 0x82, 0xff));

            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                foreach (CellIndicator indicator in CellIndicators)
                {
                    triggerSFX.PlaySound(TriggerSFX.SoundType.invalid_placement);
                    indicator.StartInvalidAnimation();
                }
            }
        }
    }
}
