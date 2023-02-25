using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public struct CodeBlockStruct
{
    public CodeBlockTypes code;
    public int[] parameters;
    public List<CodeBlockStruct> scope;
    public CodeBlockStruct(CodeBlockTypes code, int[] parameters, List<CodeBlockStruct> scope)
    {
        this.code = code;
        this.parameters = parameters;
        if (scope == null)
        {
            this.scope = null;
        }
        else
        {
            this.scope = scope;
        }

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
    [SerializeField] private List<EnemyCode> enemyCodes;

    public Dictionary<agentType, List<CodeBlockStruct>> EnemyCodeDictionary { get; private set; }
    public Dictionary<agentType, AnimatorController> EnemyAnimatorDictionary { get; private set; }

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
        EnemyAnimatorDictionary = new Dictionary<agentType, AnimatorController>();
        foreach (EnemyCode s in enemyCodes)
        {
            EnemyCodeDictionary.Add(s.type, s.code);
            EnemyAnimatorDictionary.Add(s.type, s.animatorController);
        }
    }
}
