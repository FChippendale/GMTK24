using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FactoryBehaviour : MonoBehaviour
{
    // Final score does not depend on traversal order, store a score and a flopped committedScore
    private int lastScore = 1;
    private int lastCommittedScore = 1;

    private GameObject canvas;

    public Vector2 viewportPosition;
    public GameObject text;

    public enum TraversalType
    {
        constant_integer_amount,  // ignore neighbour value, this object counts as this value
        sum_of_any_adjacent,
        largest_adjacent,
    }
    public TraversalType traversalType;

    public Color GetColor()
    {
        Dictionary<TraversalType, Color> colorMapping = new()
        {
            {TraversalType.constant_integer_amount, new Color32(106, 137, 204,255)},
            {TraversalType.largest_adjacent, new Color32(184, 233, 148,255)},
            {TraversalType.sum_of_any_adjacent, new Color32(250, 211, 144,255)},
        };
        return colorMapping[traversalType];
    }

    // ensure initial tile placed at game start is always of same type
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    public void FinalizeScore()
    {
        lastCommittedScore = lastScore;
    }

    public void ResetScore()
    {
        lastCommittedScore = 1;
        lastScore = 1;
    }

    public int GetScoreLastCalculation()
    {
        return lastCommittedScore;
    }

    public int AddScoreToCalculation(List<GameObject> neighbours)
    {
        lastScore = 0;
        switch (traversalType)
        {
            case TraversalType.constant_integer_amount:
                lastScore = 1;
                break;
            case TraversalType.sum_of_any_adjacent:
                foreach (GameObject gameObject in neighbours)
                {
                    lastScore += gameObject.GetComponent<FactoryBehaviour>().GetScoreLastCalculation();
                }
                break;
            case TraversalType.largest_adjacent:
                foreach (GameObject gameObject in neighbours)
                {
                    lastScore = Math.Max(lastScore, gameObject.GetComponent<FactoryBehaviour>().GetScoreLastCalculation());
                }
                break;
        }

        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
        Vector2 canvas_position = new(
            (viewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f),
            (viewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)
        );
        GameObject newObject = Instantiate(text, canvas.transform);
        newObject.GetComponent<RectTransform>().anchoredPosition = canvas_position - new Vector2(20.0f, 25.0f);
        if (lastScore < 0)
        {
            newObject.GetComponent<TMP_Text>().SetText(lastScore.ToString());
        }
        else if (lastScore > 0)
        {
            newObject.GetComponent<TMP_Text>().SetText("+" + lastScore.ToString());
        }
        return lastScore;
    }

}
