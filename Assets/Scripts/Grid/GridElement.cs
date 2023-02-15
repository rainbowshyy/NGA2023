using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridElement : MonoBehaviour
{
    public Vector2 gridCoords;
    public Vector2 startingCoords;
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
}
