using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScaler : MonoBehaviour
{
    public Transform target;

    public float scaleMultiplier = 0.7f;
    public float thresholdMultiplier = 2.0f;

    private int factoryCount = 0;
    private float threshold = 4.0f;

    public void FactoryAdded()
    {
        factoryCount += 1;

        if (factoryCount >= threshold)
        {
            threshold *= thresholdMultiplier;

            target.transform.localScale = target.transform.localScale * scaleMultiplier;
        }
    }
}
