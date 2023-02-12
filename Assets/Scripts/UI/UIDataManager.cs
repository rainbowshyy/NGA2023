using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    [SerializeField] private GameObject codeBlockPref;
    [SerializeField] private GameObject codeParentPref;

    [SerializeField] private RectTransform codeBlockUI;

    [SerializeField] private StructTypeUI[] typeUIStruct;
    public static Dictionary<agentType, Color> typeColor;
    public static Dictionary<agentType, string> typeName;

    public static UIDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        typeColor = new Dictionary<agentType, Color>();
        typeName = new Dictionary<agentType, string>();
        foreach (StructTypeUI s in typeUIStruct)
        {
            typeColor.Add(s.type, s.color);
            typeName.Add(s.type, s.name);
        }
    }

    private void Start()
    {
        CreateCodeParent(agentType.Blue);
        CreateCodeBlock(new MoveBlock(new int[2] { 0, 1 }));
    }

    public void CreateCodeParent(agentType type)
    {
        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go = Instantiate(codeParentPref, codeBlockUI);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlockParent comp = go.GetComponent<UICodeBlockParent>();
        comp.SetType(type);
        comp.SetDragDropBackground(codeBlockUI);
    }

    public void CreateCodeBlock(CodeBlock codeBlock)
    {
        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go = Instantiate(codeBlockPref, codeBlockUI);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlock comp = go.GetComponent<UICodeBlock>();
        comp.SetCode(codeBlock);
        comp.SetDragDropBackground(codeBlockUI);
    }
}
