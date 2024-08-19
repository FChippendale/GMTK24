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

    private bool[,,] tileToPlace = new bool[4, 4, 4];

    private GridPlacement gridPlacement;

    [SerializeField]
    private Camera sceneCamera;

    public GameObject nextPlacementHint;

    [SerializeField]
    private DeathCircle deathCircle;

    [SerializeField]
    private GameObject AutomaticPlaceParticles;

    private int numberOfColors;

    private bool TryAddTileAtGridPosition(Vector3Int gridPosition, bool allow_island = false, bool is_automatic_placement = false)
    {
        var (center_x, center_y) = gridPlacement.GetCenterTile();

        List<GameObject> gridItems = new();
        List<(int, int)> gridCoords = new();
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
        }

        if (is_automatic_placement)
        {
            triggerSFX.PlaySound(TriggerSFX.GetInvalidPlacementSound());
            AutomaticPlaceParticles.SendMessage("ZapTo", gridItems[0].transform.position);
        }
        else
        {
            triggerSFX.PlaySound(TriggerSFX.GetPlacementSound());
        }

        // Has the user scored any points?
        gridPlacement.CheckForEncirclements(FactoryBehaviour.TraversalType.constant_integer_amount);
        gridPlacement.CheckForEncirclements(FactoryBehaviour.TraversalType.largest_adjacent);
        gridPlacement.CheckForEncirclements(FactoryBehaviour.TraversalType.spray);
        gridPlacement.CheckForEncirclements(FactoryBehaviour.TraversalType.sum_of_any_adjacent);

        // Has the user died?
        foreach ((int, int) coord in unityCoords)
        {
            if (deathCircle.IsDeathTile(coord))
            {
                gameObject.SendMessage("Dead");
                return true;
            }
        }

        gameObject.SendMessage("TilesAdded", gridItems.Count);
        AssignNextFactoryType();

        return true;
    }

    void AttemptAutomaticPlacement(int attempt = 0)
    {
        // Randomly rotate piece
        int rotations = Random.Range(0, 6);
        for (int i = 0; i < rotations; i++)
        {
            RotateTileBag(true);
        }

        // Get all possible placements accounting for zeroing errors
        HashSet<(int, int)> toTry = new HashSet<(int, int)>();
        foreach (var (x, y) in gridPlacement.GetPossiblePlacements())
        {
            toTry.Add((x, y));
            toTry.Add((x + 1, y));
            toTry.Add((x + 1, y + 1));
            toTry.Add((x, y + 1));
            toTry.Add((x - 1, y));
            toTry.Add((x - 1, y - 1));
            toTry.Add((x, y - 1));
        }

        var (center_x, center_y) = gridPlacement.GetCenterTile();
        List<(int, int)> placements = new List<(int, int)>(toTry);
        placements.Sort(Comparer<(int, int)>.Create(((int, int) loc1, (int, int) loc2) =>
        {
            int lhs_x = loc1.Item1 - center_x;
            int lhs_y = loc1.Item2 - center_y;
            int rhs_x = loc2.Item1 - center_x;
            int rhs_y = loc2.Item2 - center_y;

            return (lhs_x * lhs_x + lhs_y * lhs_y).CompareTo(rhs_x * rhs_x + rhs_y * rhs_y);
        }));

        foreach (var (x, y) in placements)
        {
            if (TryAddTileAtGridPosition(new Vector3Int(x - center_x, y - center_y, 0), is_automatic_placement: true))
            {
                // Successfully added. Done!
                return;
            }
        }

        if (attempt < 4)
        {
            AttemptAutomaticPlacement(attempt + 1);
            return;
        }

        // We're stuck. End the game. The board was probably full anyway
        gameObject.SendMessage("Dead");
    }

    public void PlacementDeadlineTimerTick()
    {
        AttemptAutomaticPlacement();
    }

    private void Start()
    {
        numberOfColors = PlayerPrefs.GetInt("NumberOfColors", 4);

        tileToPlace = new bool[4, 4, 4];
        tileToPlace[0, 0, 0] = true;

        gridPlacement = GetComponent<GridPlacement>();
        factoryToPlace.GetComponent<FactoryBehaviour>().traversalType = FactoryBehaviour.TraversalType.unbreakable_starting_tile;
        TryAddTileAtGridPosition(new Vector3Int(0, 0, 0), true);
    }

    void AssignNextFactoryType()
    {
        List<FactoryBehaviour.TraversalType> traversalTypes = new()
        {
            FactoryBehaviour.TraversalType.constant_integer_amount,
            FactoryBehaviour.TraversalType.largest_adjacent,
            FactoryBehaviour.TraversalType.sum_of_any_adjacent,
            FactoryBehaviour.TraversalType.spray,
        };

        FactoryBehaviour.TraversalType type = traversalTypes[Random.Range(0, numberOfColors)];
        factoryToPlace.GetComponent<FactoryBehaviour>().traversalType = type;
        nextPlacementHint.GetComponent<Image>().color = factoryToPlace.GetComponent<FactoryBehaviour>().GetColor();

        tileToPlace = TileBag.GetRandomShape();
    }

    private void DrawPlacementHint(Vector3Int mouseGridPosition, Color color)
    {
        var (center_x, center_y) = gridPlacement.GetCenterTile();
        List<(int, int)> unityCoords = TileBag.ConvertToUnityCoords(tileToPlace, mouseGridPosition.x, mouseGridPosition.y);
        for (int i = 0; i < unityCoords.Count; i++)
        {
            CellIndicator indicator = CellIndicators[i];
            var (x, y) = unityCoords[i];

            Vector3 indicatorPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
            bool isValid = gridPlacement.grid.IsInGrid(x + center_x, y + center_y) && gridPlacement.grid.IsEmpty(x + center_x, y + center_y);

            indicator.Reposition(indicatorPosition,
                isValid ? color : CellIndicator.invalidColour);
        }
    }

    private void RotateTileBag(bool clockwise)
    {
        bool[,,] tmp = new bool[4, 4, 4];

        // TODO can we do this without nested loops?
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                for (int k = 0; k < 4; ++k)
                {
                    // Derivation in TileBag.
                    if (clockwise)
                    {
                        tmp[k, 3 - i, j] = tileToPlace[i, j, k];
                    }
                    else
                    {
                        tmp[3 - j, k, i] = tileToPlace[i, j, k];
                    }
                }
            }
        }

        tileToPlace = TileBag.Normalize(tmp);
    }

    // simple utility to move an object to the on screen position of the currently returned grid position
    private void Update()
    {
        // TODO use GetButtonDown instead to allow key remapping.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Clockwise rotation.
            triggerSFX.PlaySound(TriggerSFX.SoundType.rotate);
            RotateTileBag(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // Anti-clockwise rotation.
            triggerSFX.PlaySound(TriggerSFX.SoundType.rotate);
            RotateTileBag(false);
        }

        // get mouse grid position and convert back to screen pos
        GridPosition gridPosition = inputManager.GetSelectedGridPosition();

        if (gridPosition.type == PositionType.outside_grid)
        {
            // Cursor is not in the grid, there's nothing to do.
            return;
        }

        DrawPlacementHint(gridPosition.position, factoryToPlace.GetComponent<FactoryBehaviour>().GetColor());

        if (Input.GetMouseButtonDown((int)MouseButton.Left) && !TryAddTileAtGridPosition(gridPosition.position, false))
        {
            triggerSFX.PlaySound(TriggerSFX.GetInvalidPlacementSound());

            foreach (CellIndicator indicator in CellIndicators)
            {
                indicator.StartInvalidAnimation();
            }
        }
    }
}
