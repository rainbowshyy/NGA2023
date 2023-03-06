using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ConditionOptionType { Int, Direction}

public class ConditionOptionWindow : MonoBehaviour
{
    private UICodeBlock current;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Vector2Int[] buttonParam;
    [SerializeField] private GameObject window;
    private bool visible;

    public static Action<ConditionOptionType, UICodeBlock> onConditionOptionOpen;

    private void OnEnable()
    {
        onConditionOptionOpen += OpenConditionWindow;
    }

    private void OnDisable()
    {
        onConditionOptionOpen -= OpenConditionWindow;
    }

    private void OpenConditionWindow(ConditionOptionType type, UICodeBlock codeBlock)
    {
        visible = true;
        window.SetActive(true);
        window.transform.position = codeBlock.transform.position;
        current = codeBlock;
        switch (type)
        {
            case ConditionOptionType.Int:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].SetActive(i < 10);
                }
                break;
            case ConditionOptionType.Direction:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].SetActive(i > 9);
                }
                break;
        }
    }

    public void SetCodeParam(int id)
    {
        if (current == null)
        {
            return;
        }
        Debug.Log("pressed");

        current.Code.parameters = new int[2] { buttonParam[id].x, buttonParam[id].y };
        current.SetData();
        window.SetActive(false);
    }
}