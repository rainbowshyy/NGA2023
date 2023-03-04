using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    [SerializeField] private GameObject codeBlockPref;
    [SerializeField] private GameObject ifCodeBlockPref;
    [SerializeField] private GameObject codeBlockEnemyPref;
    [SerializeField] private GameObject ifCodeBlockEnemyPref;
    [SerializeField] private GameObject codeParentPref;
    [SerializeField] private GameObject codeParentEnemyPref;

    [SerializeField] private RectTransform[] codeBlockParents;

    public Dictionary<agentType, GameObject> EnemyCodeParents { get; private set; }
    public Dictionary<agentType, GameObject> CodeParents { get; private set; }

    public static UIDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        EnemyCodeParents = new Dictionary<agentType, GameObject>();
        CodeParents = new Dictionary<agentType, GameObject>();
    }

    private void Start()
    {
        /*
        CreateCodeBlock(new MoveBlock(new int[2] { 0, 1 }));
        CreateCodeBlock(new MoveBlock(new int[2] { 0, -1 }));
        CreateCodeBlock(new MoveBlock(new int[2] { -1, 0 }));
        CreateCodeBlock(new MoveBlock(new int[2] { 1, 0 }));
        CreateCodeBlock(new EnergyBlock(new int[2] { 1, 0 }));
        CreateCodeBlock(new EnergyGreaterBlock(new int[2] { 1, 0 }));
        CreateCodeBlock(new EnergyInRange(new int[2] { 1, 1 }));
        CreateCodeBlock(new DamageInRange(new int[2] { 1, 1 }));
        */
    }

    public void RemoveCodeParents()
    {
        foreach (GameObject codeParent in EnemyCodeParents.Values)
        {
            Destroy(codeParent);
        }
        EnemyCodeParents.Clear();
    }

    public RectTransform getParent(CodeBlock code)
    {
        if (code.isCondition)
        {
            return codeBlockParents[1];
        }
        else
        {
            return codeBlockParents[0];
        }
    }

    public void TryCreateCodeParent(agentType type)
    {
        if (CodeParents.ContainsKey(type))
        {
            return;
        }

        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject window = AgentWindowManager.Instance.AddWindow(type, false);
        GameObject go = Instantiate(codeParentPref, window.transform);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlockParent comp = go.GetComponent<UICodeBlockParent>();
        comp.SetType(type);
        comp.SetDragDropBackground(codeBlockParents[0]);    //CHANGE
        CodeParents.Add(type, go);

        comp.SetCodeForManager();

        window.GetComponent<LayoutRebuilderElement>().DoRebuild();
    }

    public void TryCreateEnemyCodeParent(agentType type)
    {
        if (EnemyCodeParents.ContainsKey(type))
        {
            return;
        }

        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject window = AgentWindowManager.Instance.AddWindow(type, true);
        GameObject go = Instantiate(codeParentEnemyPref, window.transform);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlockParent comp = go.GetComponent<UICodeBlockParent>();
        comp.SetType(type);
        comp.SetDragDropBackground(codeBlockParents[0]);

        List<CodeBlock> codeBlocks = CodeBlockManager.GetCodeListFromStruct(AgentManager.Instance.EnemyCodeMap[type]);
        comp.SetCodeBlocksInit(codeBlocks);

        EnemyCodeParents.Add(type, go);

        window.GetComponent<LayoutRebuilderElement>().DoRebuild();
    }
    public GameObject CreateCodeBlock(CodeBlock codeBlock)
    {

        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go;

        RectTransform newParent = getParent(codeBlock);

        if (codeBlock.isCondition)
        {
            go = Instantiate(ifCodeBlockPref, newParent);
        }
        else
        {
            go = Instantiate(codeBlockPref, newParent);
        }
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlock comp = go.GetComponent<UICodeBlock>();
        comp.SetCode(codeBlock);
        comp.SetDragDropBackground(newParent);

        newParent.GetComponent<LayoutRebuilderElement>().DoRebuild();

        return go;
    }

    public GameObject CreateEnemyCodeBlock(CodeBlock codeBlock)
    {
        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go;

        Transform newParent = getParent(codeBlock);

        if (codeBlock.isCondition)
        {
            go = Instantiate(ifCodeBlockEnemyPref, newParent);
        }
        else
        {
            go = Instantiate(codeBlockEnemyPref, newParent);
        }
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlock comp = go.GetComponent<UICodeBlock>();
        comp.SetCode(codeBlock);

        return go;
    }
}
