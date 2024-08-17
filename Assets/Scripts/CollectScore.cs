using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectScore : MonoBehaviour
{
    [SerializeField]
    private GameObject gridPlacementObj;
    private GridPlacement gridPlacement;

    public float TimeTillScoreCalculation = 0.0f;
    public float TimeBetweenScoreCalculations = 3.0f;
    public float TimeOfScoreCalculation = 2.0f;

    bool isCalculatingScore = false;

    void Start()
    {
        gridPlacement = gridPlacementObj.GetComponent<GridPlacement>();
    }

    void Update()
    {
        TimeTillScoreCalculation -= Time.deltaTime;
        if (TimeTillScoreCalculation < 0.0f)
        {
            isCalculatingScore = true;
            TimeTillScoreCalculation = TimeBetweenScoreCalculations;
            gridPlacement.StartScoreCalculation(TimeOfScoreCalculation);
        }

        if (isCalculatingScore)
        {
            isCalculatingScore = !gridPlacement.DoScoreCalculationUpdateStep();
        }
    }
}
