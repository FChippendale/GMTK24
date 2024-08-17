using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private RectTransform gridScreenArea;

    private Vector3Int lastPosition = new(0, 0, 0);

    // returns grid position mouse is currently over
    public Vector3Int GetSelectedGridPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        if (!RectTransformUtility.RectangleContainsScreenPoint(gridScreenArea, mousePos, sceneCamera))
        {
            return lastPosition;
        }

        Vector3 worldPos = sceneCamera.ScreenToWorldPoint(mousePos);
        Vector3Int gridPosition = grid.WorldToCell(worldPos);
        gridPosition.z = 0;
        lastPosition = gridPosition;
        return gridPosition;
    }
}
