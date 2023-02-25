using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private HashSet<Vector2Int> gridCurrentCoords;
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

        gridCurrentCoords = new HashSet<Vector2Int>();
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

    public bool TrySetGridElement(GridElement gridElement, int x, int y)
    {
        Vector2Int newCoords = new Vector2Int(x, y);

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

    public bool ElementAtTile(int x, int y)
    {
        return gridCurrentCoords.Contains(new Vector2Int(x, y));
    }

    public Vector2Int GetRandomTileInRect(int xMin, int yMin, int xMax, int yMax)
    {
        Vector2Int randomTile;

        HashSet<Vector2Int> freeTiles= new HashSet<Vector2Int>();
        for (int x = xMin; x < xMax + 1; x++)
        {
            for (int y = yMin; y < yMax + 1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height && !gridCurrentCoords.Contains(new Vector2Int(x, y)))
                {
                    freeTiles.Add(new Vector2Int(x, y));
                }
            }
        }

        System.Random rand = new System.Random();

        randomTile = freeTiles.ElementAt(rand.Next(freeTiles.Count));

        return randomTile;
    }

    public void ResetObjects()
    {
        gridCurrentCoords = new HashSet<Vector2Int>();
        foreach (GameObject g in objectsInGrid)
        {
            g.GetComponent<GridElement>().ResetPosition();
            gridCurrentCoords.Add(g.GetComponent<GridElement>().gridCoords);
        }
    }
}