using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class GridWall : GridElement
{
    public void Setup()
    {
        gridCoords = startingCoords;
        GridManager.Instance.AddGridElement(this);
    }

    public override void UpdatePosition()
    {

    }
}
