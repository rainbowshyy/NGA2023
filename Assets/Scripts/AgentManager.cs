using System.Collections;
using System.Collections.Generic;
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

public enum agentType { Cubert, Blob, Xavier, Ylvis, SirKel, Barry, Bob, RaymondLeft, RaymondRight, RaymondDown, RaymondUp, Charlie, Rayland, Hope, Treasure}

public class AgentManager : MonoBehaviour
{
    [SerializeField] private List<Agent> agents;

    public Dictionary<agentType, List<CodeBlockStruct>> EnemyCodeMap { get; private set; }
    public Dictionary<agentType, RuntimeAnimatorController> AgentAnimatorMap { get; private set; }
    public Dictionary<agentType, Color> AgentColorMap { get; private set; }
    public Dictionary<agentType, Sprite> AgentIconMap { get; private set; }
    public Dictionary<agentType, string> AgentNameMap { get; private set; }
    public Dictionary<agentType, int> AgentHealthMap { get; private set; }

    public static AgentManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        EnemyCodeMap = new Dictionary<agentType, List<CodeBlockStruct>>();
        AgentAnimatorMap = new Dictionary<agentType, RuntimeAnimatorController>();
        AgentColorMap = new Dictionary<agentType, Color>();
        AgentIconMap = new Dictionary<agentType, Sprite>();
        AgentNameMap = new Dictionary<agentType, string>();
        AgentHealthMap = new Dictionary<agentType, int>();

        foreach (Agent a in agents)
        {
            if (a.enemyCode != null && a.enemyCode.Count > 0)
            {
                EnemyCodeMap.Add(a.agentType, a.enemyCode);
            }
            AgentAnimatorMap.Add(a.agentType, a.controller);
            AgentColorMap.Add(a.agentType, a.color);
            AgentIconMap.Add(a.agentType, a.icon);
            AgentNameMap.Add(a.agentType, a.agentName);
            AgentHealthMap.Add(a.agentType, a.startingHealth);
        }
    }
}
