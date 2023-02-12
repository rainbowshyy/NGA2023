using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICodeBlock : MonoBehaviour
{
    [SerializeField] private CodeBlock code;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private DragDrop dragDrop;

    public CodeBlock Code
    {
        get { return code; }
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void SetCode(CodeBlock codeBlock)
    {
        code = codeBlock;
        SetData();
    }

    public void SetData()
    {
        SetText(code.ShowSyntax());
    }

    public void SetDragDropBackground(RectTransform rectTransform)
    {
        dragDrop.SetBackground(rectTransform);
    }
}
