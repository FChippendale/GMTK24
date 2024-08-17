using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class GridPlacement : MonoBehaviour
{
    private TileGrid grid = new TileGrid();

    public float TimePerIncrementalScoreUpdate = 0.0f;

    // Score calculation state
    public bool isCalculatingScore = false;
    float timeTillNextIncrementalScoreCalculation = 0.1f;


    int currentTraversalIndex = 0;
    List<GameObject> orderAdded = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
    }

    void doScoreCalculationUpdate()
    {
        timeTillNextIncrementalScoreCalculation -= Time.deltaTime;
        if (timeTillNextIncrementalScoreCalculation > 0.0f)
        {
            return;
        }

        // Calculate next part of score
        GameObject toCalculate = orderAdded[currentTraversalIndex];
        var (x, y) = grid.GetLocation(toCalculate);
        List<GameObject> neighbours = grid.GetNeighbours(x, y);
        toCalculate.GetComponent<FactoryBehaviour>().AddScoreToCalculation(neighbours);

        currentTraversalIndex += 1;
        if (currentTraversalIndex == orderAdded.Count)
        {
            // We're done calculating score
            foreach (GameObject obj in orderAdded)
            {
                // Reset to avoid feedback loops
                obj.GetComponent<FactoryBehaviour>().ResetScore();
            }
            currentTraversalIndex = 0;
            isCalculatingScore = false;
            return;
        }


        // We still have more updates to calculate
        timeTillNextIncrementalScoreCalculation = TimePerIncrementalScoreUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCalculatingScore)
        {
            doScoreCalculationUpdate();
        }
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
