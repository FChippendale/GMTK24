using System.Collections.Generic;
using UnityEngine;


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

    public bool TryAddToGrid(GameObject to_add, int x, int y, bool allow_island)
    {
        if (grid.TryAddTile(to_add, x, y, allow_island))
        {
            orderAdded.Add(to_add);
            return true;
        }
        return false;
    }

    public (int, int) GetCenterTile()
    {
        return grid.GetCenterTile();
    }
}
