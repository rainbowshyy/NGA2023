using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Tile[] tiles;

    public static System.Action<Vector2> onNewCenter;

    public static GridVisualizer Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        GridManager.onAdd += MoveElement;
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
        onNewCenter?.Invoke(new Vector2(GridManager.Instance.width / 2f, GridManager.Instance.height / 2f));
    }

    private void MoveElement(GridElement gridElement)
    {
        Debug.Log("a");
        gridElement.UpdatePosition();
    }
}
