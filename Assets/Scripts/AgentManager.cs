using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CodeBlockStruct : ICloneable
{
    public CodeBlockTypes code;
    [SerializeField] private int[] parameters;
    public int[] Parameters { get { return parameters; } }
    public List<CodeBlockStruct> scope;
    public int price;
    public CodeBlockStruct(CodeBlockTypes code, int[] parameters, List<CodeBlockStruct> scope, int price)
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
        this.price = price;

    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public enum agentType { Cubert, Blob, Xavier, Ylvis, SirKel, Barry, Bob, RaymondLeft, RaymondRight, RaymondDown, RaymondUp, Charlie, Rayland, Hope, Treasure}

public class AgentManager : MonoBehaviour
{
    public Dictionary<agentType, List<CodeBlockStruct>> EnemyCodeMap { get; private set; }
    public Dictionary<agentType, RuntimeAnimatorController> AgentAnimatorMap { get; private set; }
    public Dictionary<agentType, Color> AgentColorMap { get; private set; }
    public Dictionary<agentType, Sprite> AgentIconMap { get; private set; }
    public Dictionary<agentType, string> AgentNameMap { get; private set; }
    public Dictionary<agentType, int> AgentHealthMap { get; private set; }
    public Dictionary<agentType, int> AgentMusicIntensityMap { get; private set; }
    public Dictionary<agentType, int> AgentGoldMap { get; private set; }

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
        AgentMusicIntensityMap = new Dictionary<agentType, int>();
        AgentGoldMap = new Dictionary<agentType, int>();

        foreach (Agent agent in Resources.LoadAll<Agent>("Agents/"))
        {
            var a = Instantiate(agent);

            if (a.EnemyCode != null && a.EnemyCode.Count > 0)
            {
                EnemyCodeMap.Add(a.AgentType, a.EnemyCode);
            }
            AgentAnimatorMap.Add(a.AgentType, a.Controller);
            AgentColorMap.Add(a.AgentType, a.Color);
            AgentIconMap.Add(a.AgentType, a.Icon);
            AgentNameMap.Add(a.AgentType, a.AgentName);
            AgentHealthMap.Add(a.AgentType, a.StartingHealth);
            AgentMusicIntensityMap.Add(a.AgentType, a.MusicIntensity);
            AgentGoldMap.Add(a.AgentType, a.Gold);
        }
    }
}
