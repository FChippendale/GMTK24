using System.Collections.Generic;
using UnityEngine;


public class FactoryBehaviour : MonoBehaviour
{
    public enum TraversalType
    {
        constant_integer_amount,  // ignore neighbour value, this object counts as this value
        sum_of_any_adjacent,
        largest_adjacent,
        spray,
        unbreakable_starting_tile,
    }
    public TraversalType traversalType;

    public Color GetColor()
    {
        Dictionary<TraversalType, Color> colorMapping = new()
        {
            {TraversalType.constant_integer_amount, new Color32(106, 137, 204, 255)},
            {TraversalType.largest_adjacent, new Color32(184, 233, 148, 255)},
            {TraversalType.sum_of_any_adjacent, new Color32(250, 211, 144, 255)},
            {TraversalType.spray, new Color32(130, 204, 221, 255)},
            {TraversalType.unbreakable_starting_tile, new Color32(10, 61, 98, 161)}
        };
        return colorMapping[traversalType];
    }
}
