using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Agent", menuName = "Data/Create new Agent")]
public class Agent : ScriptableObject
{
    public agentType agentType; //unique
    public Sprite icon;
    public RuntimeAnimatorController controller;
    public List<CodeBlockStruct> enemyCode;
    public string agentName;
    public Color color;
    public int startingHealth;
}
