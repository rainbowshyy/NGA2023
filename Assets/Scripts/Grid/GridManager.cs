using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private HashSet<Vector2> gridCurrentCoords;

    public int width;
    public int height;

    public static GridManager Instance { get; private set; }

    public static System.Action<GridElement> onRemove;
    public static System.Action<GridElement> onAdd;
    public static System.Action<GridElement, Vector2> onMove;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridCurrentCoords = new HashSet<Vector2>();
    }

    public bool AddGridElement(GridElement gridElement)
    {
        if (gridCurrentCoords.Contains(gridElement.gridCoords))
        {
            return false;
        }
        gridCurrentCoords.Add(gridElement.gridCoords);
        onAdd?.Invoke(gridElement);
        return true;
    }

    public bool TryMoveGridElement(GridElement gridElement, int x, int y)
    {
        Vector2 newCoords = new Vector2(gridElement.gridCoords.x + x, gridElement.gridCoords.y + y);

        string debug = "";
        foreach (Vector2 g in gridCurrentCoords)
        {
            debug += "(" + g.x + ", " + g.y + "), ";
        }
        Debug.Log(debug);

        if (gridCurrentCoords.Contains(newCoords))
            Debug.Log("coll");

        if (
            gridCurrentCoords.Contains(newCoords) ||
            newCoords.x < 0 ||
            newCoords.x >= width ||
            newCoords.y < 0 ||
            newCoords.y >= height
            )
        {
            return false;
        }

        gridCurrentCoords.Remove(gridElement.gridCoords);
        gridCurrentCoords.Add(newCoords);
        gridElement.gridCoords = newCoords;
        onMove?.Invoke(gridElement, newCoords);

        return true;
    }
}