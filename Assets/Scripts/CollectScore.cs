using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectScore : MonoBehaviour
{
    [SerializeField]
    private GameObject gridPlacementObj;

    public float TimeTillScoreCalculation = 0.0f;
    public float TimeBetweenScoreCalculations = 0.5f;

    void Start()
    {

    }

    void Update()
    {
        TimeTillScoreCalculation -= Time.deltaTime;
        if (TimeTillScoreCalculation < 0.0f)
        {
            gridPlacementObj.GetComponent<GridPlacement>().isCalculatingScore = true;
            TimeTillScoreCalculation = TimeBetweenScoreCalculations;
        }
    }
}
