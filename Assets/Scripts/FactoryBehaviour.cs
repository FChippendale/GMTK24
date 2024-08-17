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


    public enum TraversalType
    {
        constant_integer_amount,
        sum_of_any_adjacent,
        largest_adjacent,
    }
    public TraversalType traversalType;

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
    }

}
