using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RightClickWindow : MonoBehaviour
{
    private agentType type;
    private bool mouseOver;

    private void OnEnable()
    {
        InputManager.onRightMouse += ShowWindow;
    }

    private void OnDisable()
    {
        InputManager.onRightMouse -= ShowWindow;
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }

    private void ShowWindow(bool press)
    {
        if (!press || !mouseOver)
        {
            return;
        }

        type = GetComponent<CodeBlockAgent>().type;

        AgentWindowManager.Instance.OpenWindow(type, true);
    }
}
