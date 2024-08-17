using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


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

    // ensure initial tile placed at game start is always of same type
    void Start()
    {
        traversalType = TraversalType.constant_integer_amount;
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

    public void AddScoreToCalculation(List<GameObject> neighbours)
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
        Vector2 canvas_position = new Vector2(
            (viewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f),
            (viewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)
        );
        GameObject newObject = GameObject.Instantiate(text, canvas.transform);
        newObject.GetComponent<RectTransform>().anchoredPosition = canvas_position - new Vector2(20.0f, 25.0f);
    }

}
