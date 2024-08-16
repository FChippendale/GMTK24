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
    bool isCalculatingScore = false;
    float timeTillNextIncrementalScoreCalculation = 0.1f;

    HashSet<GameObject> alreadyCalculated = new HashSet<GameObject>();
    List<GameObject> currentlyCalculating = new List<GameObject>();


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
        List<GameObject> nextToCalculate = new List<GameObject>();

        foreach (GameObject obj in currentlyCalculating)
        {
            var (x, y) = grid.GetLocation(obj);
            List<GameObject> neighbours = grid.GetNeighbours(x, y);

            obj.GetComponent<FactoryBehaviour>().AddScoreToCalculation(neighbours);

            foreach (GameObject neighbour in neighbours)
            {
                if (!alreadyCalculated.Contains(neighbour))
                {
                    nextToCalculate.Add(neighbour);
                    alreadyCalculated.Add(neighbour);
                }
            }
        }
        foreach (GameObject obj in currentlyCalculating)
        {
            obj.GetComponent<FactoryBehaviour>().FinalizeScore();
        }

        if (nextToCalculate.Count == 0)
        {
            // We're done calculating score
            foreach (GameObject obj in alreadyCalculated)
            {
                // Reset to avoid feedback loops
                obj.GetComponent<FactoryBehaviour>().ResetScore();
            }
            alreadyCalculated.Clear();
            currentlyCalculating.Clear();
            isCalculatingScore = false;
            return;
        }


        // We still have more updates to calculate
        currentlyCalculating = nextToCalculate;
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
        return grid.TryAddTile(to_add, x, y, allow_island);
    }

    public (int, int) GetCenterTile()
    {
        return grid.GetCenterTile();
    }
}
