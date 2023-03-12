using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    public Grid grid;

    [SerializeField] private Tile[] tiles;

    public static System.Action onNewCenter;

    public static GridVisualizer Instance;

    public float xOffset { get; private set; }
    public float yOffset { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GridManager.onAdd += InitElement;
        GridManager.onMove += MoveElement;
        GridManager.onRemove += RemoveElement;
        GameManager.onNewStage += ChangeColor;
    }

    private void OnDisable()
    {
        GridManager.onMove -= MoveElement;
        GridManager.onAdd -= InitElement;
        GridManager.onRemove -= RemoveElement;
        GameManager.onNewStage -= ChangeColor;
    }

    private void Start()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        for (int x = 0; x < GridManager.Instance.width; x++)
        {
            for (int y = 0; y < GridManager.Instance.height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[0]);
            }
        }
        UpdateCenter();
    }

    private void UpdateCenter()
    {
        xOffset = GridManager.Instance.width / -2f;
        yOffset = GridManager.Instance.height / -2f + 0.1875f;

        onNewCenter?.Invoke();
        grid.transform.position = new Vector3(xOffset, yOffset, 0);
    }

    private void ChangeColor(Stages stage)
    {
        tilemap.color = GameManager.Instance.actColor[(int)stage] * 0.9f;
    }

    private void MoveElement(GridElement gridElement, Vector2 coords)
    {
        gridElement.UpdatePosition();
        if (gridElement is GridWall)
        {
            tilemap.SetTile(new Vector3Int((int)coords.x, (int)coords.y, 0), tiles[1]);
        }
    }

    private void InitElement(GridElement gridElement)
    {
        gridElement.UpdatePosition();
        if (gridElement is GridWall)
        {
            tilemap.SetTile(new Vector3Int(gridElement.startingCoords.x, gridElement.startingCoords.y, 0), tiles[1]);
        }
    }

    private void RemoveElement(GridElement gridElement)
    {
        if (gridElement is GridWall)
        {
            tilemap.SetTile(new Vector3Int(gridElement.gridCoords.x, gridElement.gridCoords.y, 0), tiles[0]);
        }
    }

    public Vector3 GetWorldPos(Vector2Int pos)
    {
        Vector3 newPos;

        newPos = grid.GetCellCenterWorld((Vector3Int)pos);

        return newPos;
    }
}
