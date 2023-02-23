using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CodeBlockStruct
{
    public CodeBlockTypes code;
    public int[] parameters;
    public CodeBlockStruct(CodeBlockTypes code, int[] parameters)
    {
        this.code = code;
        this.parameters = parameters;
    }
}

[System.Serializable]
public struct EnemyCodeStruct
{
    public agentType type;
    public List<CodeBlockStruct> codeBlocks;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyCodeStruct> enemyCodeDictionaryStructs;

    public Dictionary<agentType, List<CodeBlockStruct>> EnemyCodeDictionary { get; private set; }

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        EnemyCodeDictionary = new Dictionary<agentType, List<CodeBlockStruct>>();
        foreach (EnemyCodeStruct s in enemyCodeDictionaryStructs)
        {
            EnemyCodeDictionary.Add(s.type, s.codeBlocks);
        }
    }
}
