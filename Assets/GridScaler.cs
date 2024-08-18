using System;
using UnityEngine;

public class GridScaler : MonoBehaviour
{
    public Transform target;

    public float scaleMultiplier = 0.7f;
    public float thresholdMultiplier = 2.0f;
    public float animationRate = 0.3f;
    public float shrinkSlack = 0.9f;

    private int factoryCount = 0;
    private float threshold = 4.0f;
    private Vector3 initialScale;
    private float currentMultiplier = 1.0f;
    private float targetMultiplier = 1.0f;

    void Start()
    {
        initialScale = target.transform.localScale;
    }


    void Update()
    {
        currentMultiplier = Mathf.MoveTowards(currentMultiplier,
            targetMultiplier, animationRate * Time.deltaTime);
        target.transform.localScale = initialScale * currentMultiplier;
    }

    public void BreakingTiles(int count)
    {
        factoryCount -= count;
    }

    public void FactoryAdded()
    {
        factoryCount += 4;

        if (factoryCount >= threshold)
        {
            threshold *= thresholdMultiplier;
            targetMultiplier *= scaleMultiplier;
        }
    }
}
