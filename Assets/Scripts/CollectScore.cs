using System;
using TMPro;
using UnityEngine;

public class CollectScore : MonoBehaviour
{
    public TextMeshProUGUI text;

    [SerializeField]
    private int score = 0;

    private void Start()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        text.SetText("{}", score);
    }

    public void BreakingTiles((int, int) info)
    {
        var (count, encirclement_count) = info;
        score += 1000 * (count + encirclement_count * encirclement_count);
        UpdateScore();
    }

    public void TilesAdded(int count)
    {
        score += 100 * count;
        UpdateScore();
    }
}
