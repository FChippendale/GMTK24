using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectScore : MonoBehaviour
{
    [SerializeField]
    private GameObject gridPlacementObj;
    [SerializeField]
    private GameObject measuringScaleObj;

    [SerializeField]
    private GridPlacement gridPlacement;

    float timeTillScoreCalculation = 0.0f;
    public float TimeBetweenScoreCalculations = 3.0f;
    public float TimeOfScoreCalculation = 2.0f;


    float timeTillTax = 0.0f;
    [SerializeField]
    int numberOfTaxes = 0;
    public float TimeBetweenTax = 2.0f;


    bool isCalculatingScore = false;

    void Start()
    {
        numberOfTaxes = 0;
    }

    void Update()
    {
    }
}
