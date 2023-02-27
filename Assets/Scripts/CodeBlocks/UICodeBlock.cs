using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UICodeBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CodeBlock code;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private DragDrop dragDrop;

    private bool hover;
    private bool showed;
    private float hoverTimeCurrent;
    [SerializeField] private float hoverTimeNeeded;

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

    private void Update()
    {
        if (hover)
        {
            if (hoverTimeCurrent < hoverTimeNeeded)
            {
                hoverTimeCurrent += Time.deltaTime;
            }
            else if (!showed)
            {
                TooltipBox.onShowTooltip?.Invoke(transform.position, code.ToolTip());
                showed = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
        showed = false;
        hoverTimeCurrent = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        TooltipBox.onHideTooltip?.Invoke();
        hoverTimeCurrent = 0;
    }
}
