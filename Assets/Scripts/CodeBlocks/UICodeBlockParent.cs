using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICodeBlockParent : MonoBehaviour
{
    [SerializeField] private agentType type;

    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform codeParent;

    [SerializeField] private DragDrop dragDrop;

    public agentType Type
    {
        get { return type; }
    }

    public Image Sprite
    {
        get { return sprite; }
    }

    private void Awake()
    {
        DragDropManager.onStopDrag += SetCodeForManager;
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }

    public void SetText(string newText)
    {
        text.text = newText + " ( )";
    }

    public void SetType (agentType newType)
    {
        type = newType;
        SetData();
    }

    public void SetData()
    {
        SetColor(UIDataManager.typeColor[type]);
        SetText(UIDataManager.typeName[type]);
    }

    public void SetDragDropBackground(RectTransform rectTransform)
    {
        dragDrop.SetBackground(rectTransform);
    }

    public List<CodeBlock> GetCodeBlocks()
    {
        List<CodeBlock> current = new List<CodeBlock>();

        foreach (Transform t in codeParent)
        {
            UICodeBlock currentUICodeBlock = t.gameObject.GetComponent<UICodeBlock>();

            currentUICodeBlock.UpdateCodeBlockScope();

            current.Add(currentUICodeBlock.Code);
        }

        return current;
    }

    public void SetCodeForManager()
    {
        CodeBlockManager.Instance.SetCodeForType(type, GetCodeBlocks());
    }

    public void SetCodeBlocks(List<CodeBlock> code)
    {
        foreach (CodeBlock c in code)
        {
            GameObject go = UIDataManager.Instance.CreateEnemyCodeBlock(c);
            go.transform.SetParent(codeParent);
        }
        SetCodeForManager();
    }
}