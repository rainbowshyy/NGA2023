using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    [SerializeField] private GameObject codeBlockPref;
    [SerializeField] private GameObject ifCodeBlockPref;
    [SerializeField] private GameObject codeBlockEnemyPref;
    [SerializeField] private GameObject codeParentPref;
    [SerializeField] private GameObject codeParentEnemyPref;

    [SerializeField] private RectTransform codeBlockUI;

    [SerializeField] private StructTypeUI[] typeUIStruct;
    public static Dictionary<agentType, Color> typeColor;
    public static Dictionary<agentType, string> typeName;

    public HashSet<agentType> CodeParents { get; private set; }

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

        CodeParents = new HashSet<agentType>();
    }

    private void Start()
    {
        CreateCodeBlock(new MoveBlock(new int[2] { 0, 1 }));
        CreateCodeBlock(new MoveBlock(new int[2] { 0, -1 }));
        CreateCodeBlock(new MoveBlock(new int[2] { -1, 0 }));
        CreateCodeBlock(new MoveBlock(new int[2] { 1, 0 }));
        CreateCodeBlock(new EnergyBlock(new int[2] { 1, 0 }));
        CreateCodeBlock(new EnergyGreaterBlock(new int[2] { 1, 0 }));
    }

    public void TryCreateCodeParent(agentType type)
    {
        if (CodeParents.Contains(type))
        {
            return;
        }

        CodeParents.Add(type);

        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go = Instantiate(codeParentPref, codeBlockUI);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlockParent comp = go.GetComponent<UICodeBlockParent>();
        comp.SetType(type);
        comp.SetDragDropBackground(codeBlockUI);
    }

    public void TryCreateEnemyCodeParent(agentType type)
    {
        if (CodeParents.Contains(type))
        {
            return;
        }

        CodeParents.Add(type);

        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go = Instantiate(codeParentEnemyPref, codeBlockUI);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlockParent comp = go.GetComponent<UICodeBlockParent>();
        comp.SetType(type);
        comp.SetDragDropBackground(codeBlockUI);

        List<CodeBlock> codeBlocks = CodeBlockManager.Instance.GetCodeFromStruct(EnemyManager.Instance.EnemyCodeDictionary[type]);
        comp.SetCodeBlocks(codeBlocks);
    }
    public GameObject CreateCodeBlock(CodeBlock codeBlock)
    {
        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go;

        if (codeBlock.isCondition)
        {
            go = Instantiate(ifCodeBlockPref, codeBlockUI);
        }
        else
        {
            go = Instantiate(codeBlockPref, codeBlockUI);
        }
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlock comp = go.GetComponent<UICodeBlock>();
        comp.SetCode(codeBlock);
        comp.SetDragDropBackground(codeBlockUI);
        return go;
    }

    public GameObject CreateEnemyCodeBlock(CodeBlock codeBlock)
    {
        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go = Instantiate(codeBlockEnemyPref, codeBlockUI);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlock comp = go.GetComponent<UICodeBlock>();
        comp.SetCode(codeBlock);
        return go;
    }
}
