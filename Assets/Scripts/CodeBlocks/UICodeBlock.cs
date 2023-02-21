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

    public Transform codeParent;

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

    private List<UICodeBlock> GetScope()
    {
        List<UICodeBlock> scope = new List<UICodeBlock>();

        if (codeParent != null)
        {
            foreach (Transform t in codeParent)
            {
                scope.Add(t.gameObject.GetComponent<UICodeBlock>());
            }
        }

        return scope;
    }

    public void UpdateCodeBlockScope()
    {
        code.scope = new List<CodeBlock>();

        foreach (UICodeBlock u in GetScope())
        {
            u.UpdateCodeBlockScope();
            code.scope.Add(u.Code);
        }
    }
}
