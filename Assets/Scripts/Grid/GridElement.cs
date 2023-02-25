using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridElement : MonoBehaviour
{
    public Vector2Int gridCoords;
    public Vector2Int startingCoords;
    public AgentUI UI;

    public virtual void Start()
    {
        gridCoords = startingCoords;
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

    public void SetStartingCoords(Vector2Int pos)
    {
        startingCoords = pos;
    }

    public void ResetPosition()
    {
        gridCoords = startingCoords;
        UpdatePosition();
    }
}
