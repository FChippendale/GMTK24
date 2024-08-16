using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class GridPlacement : MonoBehaviour
{
    public TileGrid Grid;

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
            // TODO obj.calculate score here!!

            var (x, y) = Grid.GetLocation(obj);
            foreach (GameObject neighbour in Grid.GetNeighbours(x, y))
            {
                if (!alreadyCalculated.Contains(neighbour))
                {
                    nextToCalculate.Add(neighbour);
                    alreadyCalculated.Add(neighbour);
                }
            }
        }

        if (nextToCalculate.Count == 0)
        {
            // We're done calculating score
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

    public bool TryAddToGrid(GameObject to_add, int x, int y)
    {
        return Grid.TryAddTile(to_add, x, y);
    }
}
