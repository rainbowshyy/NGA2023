using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    [SerializeField] private GameObject codeBlockPref;
    [SerializeField] private GameObject ifCodeBlockPref;
    [SerializeField] private GameObject codeBlockEnemyPref;
    [SerializeField] private GameObject ifCodeBlockEnemyPref;
    [SerializeField] private GameObject codeParentPref;
    [SerializeField] private GameObject codeParentEnemyPref;

    [SerializeField] private RectTransform codeBlockUI;

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

    public void TryCreateCodeParent(agentType type)
    {
        if (CodeParents.ContainsKey(type))
        {
            return;
        }

        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go = Instantiate(codeParentPref, codeBlockUI);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlockParent comp = go.GetComponent<UICodeBlockParent>();
        comp.SetType(type);
        comp.SetDragDropBackground(codeBlockUI);
        CodeParents.Add(type, go);
    }

    public void TryCreateEnemyCodeParent(agentType type)
    {
        if (EnemyCodeParents.ContainsKey(type))
        {
            return;
        }

        Vector3 pos = new Vector3(450f + Random.Range(-200f, 200f), -260f + Random.Range(-100f, 100f), 0);

        GameObject go = Instantiate(codeParentEnemyPref, codeBlockUI);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlockParent comp = go.GetComponent<UICodeBlockParent>();
        comp.SetType(type);
        comp.SetDragDropBackground(codeBlockUI);

        List<CodeBlock> codeBlocks = CodeBlockManager.GetCodeListFromStruct(AgentManager.Instance.EnemyCodeMap[type]);
        comp.SetCodeBlocksInit(codeBlocks);

        EnemyCodeParents.Add(type, go);
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

        GameObject go;

        if (codeBlock.isCondition)
        {
            go = Instantiate(ifCodeBlockEnemyPref, codeBlockUI);
        }
        else
        {
            go = Instantiate(codeBlockEnemyPref, codeBlockUI);
        }
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        UICodeBlock comp = go.GetComponent<UICodeBlock>();
        comp.SetCode(codeBlock);
        return go;
    }
}
