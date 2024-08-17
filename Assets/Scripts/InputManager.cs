using UnityEngine;

public enum PositionType
{
    valid,
    invalid,
    outside_grid
}

public struct GridPosition
{
    public Vector3Int position;
    public PositionType type;
}

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private RectTransform validGameArea;
    [SerializeField]
    private RectTransform gameArea;


    // returns grid position mouse is currently over
    public GridPosition GetSelectedGridPosition()
    {
        GridPosition result;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = sceneCamera.ScreenToWorldPoint(mousePos);
        result.position = grid.WorldToCell(worldPos);
        result.position.z = 0;

        bool inGrid = RectTransformUtility.RectangleContainsScreenPoint(validGameArea, mousePos, sceneCamera);
        bool inGameArea = RectTransformUtility.RectangleContainsScreenPoint(gameArea, mousePos, sceneCamera);

        result.type = inGrid ? PositionType.valid :
                        inGameArea ? PositionType.invalid :
                        PositionType.outside_grid;

        return result;
    }
}
