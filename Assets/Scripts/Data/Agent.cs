using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Agent", menuName = "Data/Create new Agent")]
public class Agent : ScriptableObject
{
    [SerializeField] private agentType agentType; //unique
    [SerializeField] private Sprite icon;
    [SerializeField] private RuntimeAnimatorController controller;
    [SerializeField] private List<CodeBlockStruct> enemyCode;
    [SerializeField] private string agentName;
    [SerializeField] private Color color;
    [SerializeField] private int startingHealth;
    [SerializeField] private int musicIntensity;
    [SerializeField] private int gold;
    public agentType AgentType => agentType;
    public Sprite Icon => icon;
    public string AgentName => agentName;
    public Color Color => color;
    public int StartingHealth => startingHealth;
    public int Gold => gold;
    public int MusicIntensity => musicIntensity;
    public RuntimeAnimatorController Controller => controller;
    public List<CodeBlockStruct> EnemyCode => enemyCode;
}
