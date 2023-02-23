using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropGrid : MonoBehaviour
{
    private bool mouseOver = false;
    private Vector2Int tileCurrent;
    private bool inputsEnabled;
    private bool dragged = false;

    private void OnEnable()
    {
        InputManager.onLeftMouse += TryPickUp;
        InputManager.onToggleInputs += ToggleInputsEnabled;
        ToggleInputsEnabled(true);
    }

    private void OnDisable()
    {
        InputManager.onLeftMouse -= TryPickUp;
        InputManager.onToggleInputs-= ToggleInputsEnabled;
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }

    private void ToggleInputsEnabled(bool enabled)
    {
        inputsEnabled = enabled;
    }

    private void TryPickUp(bool press)
    {
        if (!inputsEnabled)
        {
            return;
        }

        if (press && mouseOver)
        {
            InputManager.onMousePos += Move;
            dragged = true;
        }
        else if (!press && dragged)
        {
            dragged = false;
            InputManager.onMousePos -= Move;
            if (tileCurrent.y < 2)
            {
                GridManager.Instance.TrySetGridElement(GetComponent<GridElement>(), tileCurrent.x, tileCurrent.y);
            }
            GetComponent<GridElement>().UpdatePosition();
        }
    }

    private void Move(Vector2 pos)
    {
        Vector3 newPos = CameraManager.Instance.cam.ScreenToWorldPoint(pos);
        newPos.z = 0;
        transform.position = newPos;

        tileCurrent = (Vector2Int)GridVisualizer.Instance.grid.WorldToCell(newPos);
    }
}
