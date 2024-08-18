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

    public void BreakingTiles(int count)
    {
        score += count * count;
        UpdateScore();
    }

    public void TilesAdded(int count)
    {
        score += count;
        UpdateScore();
    }
}
