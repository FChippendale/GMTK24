using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridScaler : MonoBehaviour
{
    public Transform target;

    public float scaleMultiplier = 0.68f;
    public float thresholdMultiplier = 2.0f;
    public float animationRate = 0.3f;
    public int thresholdCount = 6;

    private int tileCount = 0;
    private readonly List<float> thresholds = new();
    private Vector3 initialScale;
    private float currentMultiplier = 1.0f;
    [SerializeField]
    private float targetMultiplier = 1.0f;

    void Start()
    {
        initialScale = target.transform.localScale;

        thresholds.Add(4.0f);
        for (int i = 0; i < thresholdCount - 1; ++i)
        {
            thresholds.Add(thresholds.Last() * thresholdMultiplier);
        }
    }

    void Update()
    {
        currentMultiplier = Mathf.MoveTowards(currentMultiplier,
            targetMultiplier, animationRate * Time.deltaTime);
        target.transform.localScale = initialScale * currentMultiplier;
    }

    void UpdateScale()
    {
        int i = 0;
        for (; i < thresholds.Count; ++i)
        {
            if (tileCount < thresholds[i]) { break; }
        }

        targetMultiplier = Mathf.Pow(scaleMultiplier, i);
    }

    public void BreakingTiles((int, int) info)
    {
        var (count, _) = info;
        tileCount -= count;

        UpdateScale();
    }

    public void TilesAdded(int count)
    {
        tileCount += count;

        UpdateScale();
    }
}
