using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private HashSet<Vector2> gridCurrentCoords;
    private List<GameObject> objectsInGrid;

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
        objectsInGrid = new List<GameObject>();
    }

    public bool AddGridElement(GridElement gridElement)
    {
        if (gridCurrentCoords.Contains(gridElement.gridCoords))
        {
            return false;
        }
        gridCurrentCoords.Add(gridElement.gridCoords);
        objectsInGrid.Add(gridElement.gameObject);
        onAdd?.Invoke(gridElement);
        return true;
    }

    public void RemoveGridElement(GridElement gridElement)
    {
        if (gridCurrentCoords.Contains(gridElement.gridCoords))
        {
            gridCurrentCoords.Remove(gridElement.gridCoords);
        }
        if (objectsInGrid.Contains(gridElement.gameObject))
        {
            objectsInGrid.Remove(gridElement.gameObject);
        }
        onRemove?.Invoke(gridElement);
    }

    public CodeBlockAgent GetAgentAtCoords(int x, int y)
    {
        CodeBlockAgent agent = null;

        foreach (GameObject g in objectsInGrid)
        {
            GridElement comp = g.GetComponent<GridElement>();

            if (comp.gridCoords.x == x && comp.gridCoords.y == y)
            {
                agent = g.GetComponent<CodeBlockAgent>();
                break;
            }
        }

        return agent;
    }

    public List<CodeBlockAgent> GetAgentsInRange(int x, int y, int range, bool includeCenter)
    {
        List<CodeBlockAgent> agents = new List<CodeBlockAgent>();

        foreach (GameObject g in objectsInGrid)
        {
            GridElement comp = g.GetComponent<GridElement>();

            if (comp.gridCoords.x >= x - range &&
                comp.gridCoords.x <= x + range &&
                comp.gridCoords.y >= y - range &&
                comp.gridCoords.y <= y + range)
            {
                if (includeCenter || comp.gridCoords.x != x || comp.gridCoords.y != y)
                {
                    agents.Add(g.GetComponent<CodeBlockAgent>());
                }
            }
        }

        return agents;
    }

    public bool TryMoveGridElement(GridElement gridElement, int x, int y)
    {
        Vector2Int newCoords = new Vector2Int(gridElement.gridCoords.x + x, gridElement.gridCoords.y + y);

        /*
        string debug = "";
        foreach (Vector2 g in gridCurrentCoords)
        {
            debug += "(" + g.x + ", " + g.y + "), ";
        }
        Debug.Log(debug);
        */

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