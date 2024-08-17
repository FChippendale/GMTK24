using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    [SerializeField]
    private Button FactoryButton0, FactoryButton1, FactoryButton2;

    public FactoryBehaviour factoryToPlace;

    // Start is called before the first frame update
    void Start()
    {
        FactoryButton0.onClick.AddListener(() => ButtonClicked(FactoryBehaviour.TraversalType.constant_integer_amount));
        FactoryButton1.onClick.AddListener(() => ButtonClicked(FactoryBehaviour.TraversalType.sum_of_any_adjacent));
        FactoryButton2.onClick.AddListener(() => ButtonClicked(FactoryBehaviour.TraversalType.largest_adjacent));
    }

    void ButtonClicked(FactoryBehaviour.TraversalType newTraversalType)
    {
        factoryToPlace.traversalType = newTraversalType;
    }
}
