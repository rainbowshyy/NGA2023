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
        transform.position = new Vector3(gridCoords.x + 0.5f, gridCoords.y + 0.5f, 0);
    }

    public bool TryMove(int x, int y)
    {
        return GridManager.Instance.TryMoveGridElement(this, x, y);
    }
}
