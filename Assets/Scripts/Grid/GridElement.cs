using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCoordinates
{
    public int x;
    public int y;
    public GridCoordinates(int xParam, int yParam)
    {
        x = xParam;
        y = yParam;
    }
}

public abstract class GridElement : MonoBehaviour
{
    public GridCoordinates gridCoords;
    [SerializeField] private GridCoordinates startingCoords;
    public AgentUI UI;

    public virtual void Awake()
    {
        gridCoords = startingCoords;
    }

    public virtual void Start()
    {
        if (!GridManager.Instance.AddGridElement(this))
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePosition()
    {
        transform.position = new Vector3(GridVisualizer.Instance.xOffset + gridCoords.x + 0.5f, GridVisualizer.Instance.yOffset + gridCoords.y + 0.5625f, 0);
        if (UI != null)
        {
            UI.SetPositionFromWorld(transform.position);
        }
    }

    public bool TryMove(int x, int y)
    {
        return GridManager.Instance.TryMoveGridElement(this, x, y);
    }
}
