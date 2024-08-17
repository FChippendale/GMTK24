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
        FactoryButton0.onClick.AddListener(() => ButtonClicked(FactoryButton0, FactoryBehaviour.TraversalType.constant_integer_amount));
        FactoryButton1.onClick.AddListener(() => ButtonClicked(FactoryButton1, FactoryBehaviour.TraversalType.sum_of_any_adjacent));
        FactoryButton2.onClick.AddListener(() => ButtonClicked(FactoryButton2, FactoryBehaviour.TraversalType.largest_adjacent));

        ButtonClicked(FactoryButton2, FactoryBehaviour.TraversalType.largest_adjacent);
    }

    void ButtonClicked(Button button, FactoryBehaviour.TraversalType newTraversalType)
    {
        factoryToPlace.traversalType = newTraversalType;
        factoryToPlace.factoryColor = button.GetComponent<Image>().color;
    }
}
