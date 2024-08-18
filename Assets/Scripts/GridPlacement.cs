using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridPlacement : MonoBehaviour
{
    public MeasuringScale measuringScale;

    private readonly TileGrid grid = new();

    float TimePerIncrementalScoreUpdate = 0.0f;
    float timeTillNextIncrementalScoreCalculation = 0.1f;


    int currentTraversalIndex = 0;
    readonly List<GameObject> orderAdded = new();


    public void StartScoreCalculation(float targetTime)
    {
        currentTraversalIndex = 0;
        TimePerIncrementalScoreUpdate = targetTime / orderAdded.Count;
        timeTillNextIncrementalScoreCalculation = 0;
    }

    public bool DoScoreCalculationUpdateStep()
    {
        timeTillNextIncrementalScoreCalculation -= Time.deltaTime;
        if (timeTillNextIncrementalScoreCalculation > 0.0f)
        {
            return false;
        }

        // Calculate next part of score
        GameObject toCalculate = orderAdded[currentTraversalIndex];
        var (x, y) = grid.GetLocation(toCalculate);
        List<GameObject> neighbours = grid.GetNeighbours(x, y);
        int score = toCalculate.GetComponent<FactoryBehaviour>().AddScoreToCalculation(neighbours);
        measuringScale.SendMessage("FactoryScoreUpdate", score);

        currentTraversalIndex += 1;
        if (currentTraversalIndex == orderAdded.Count)
        {
            // We're done calculating score
            foreach (GameObject obj in orderAdded)
            {
                // Reset to avoid feedback loops
                obj.GetComponent<FactoryBehaviour>().ResetScore();
            }
            return true;
        }

        timeTillNextIncrementalScoreCalculation = TimePerIncrementalScoreUpdate;
        return false;
    }

    public List<(int, int)> GetPossiblePlacements()
    {
        return grid.GetPossiblePlacements();
    }

    public bool TryAddToGrid(List<GameObject> to_add, List<(int, int)> positions, bool allow_island)
    {
        if (grid.TryAddTile(to_add, positions, allow_island))
        {
            foreach (GameObject obj in to_add)
            {
                orderAdded.Add(obj);
            }
            List<(int, int)> topology = grid.GetTilesEncircledBy(to_add[0].GetComponent<FactoryBehaviour>().traversalType);
            Debug.Log(topology.Count);
            return true;
        }
        return false;
    }

    public (int, int) GetCenterTile()
    {
        return grid.GetCenterTile();
    }

    public void Update()
    {
        // Get enclosed space
    }
}
