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

        GridManager.onAdd += InitElement;
        GridManager.onMove += MoveElement;
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
        yOffset = GridManager.Instance.height / -2f;

        onNewCenter?.Invoke();
        grid.transform.position = new Vector3(xOffset, yOffset, 0);
    }

    private void MoveElement(GridElement gridElement, Vector2 coords)
    {
        gridElement.UpdatePosition();
    }

    private void InitElement(GridElement gridElement)
    {
        gridElement.UpdatePosition();
    }

    public Vector3 GetWorldPos(Vector2Int pos)
    {
        Vector3 newPos;

        newPos = grid.GetCellCenterWorld((Vector3Int)pos);

        return newPos;
    }
}
