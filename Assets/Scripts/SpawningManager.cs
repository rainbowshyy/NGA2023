using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPref;

    [SerializeField] private GameObject agentPref;

    [SerializeField] private GameObject agentUIPref;
    [SerializeField] private GameObject enemyUIPref;

    [SerializeField] private Transform agentParent;
    [SerializeField] private Transform agentUIParent;

    public static SpawningManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SpawnCodeAgent(agentType type, Vector2 pos)
    {
        UIDataManager.Instance.TryCreateCodeParent(type);

        GameObject go = Instantiate(agentPref, agentParent);
        CodeBlockAgent codeAgent = go.GetComponent<CodeBlockAgent>();
        codeAgent.startingCoords = pos;
        codeAgent.type = type;


        GameObject UI = Instantiate(agentUIPref, agentUIParent);
        AgentUI agentUI = UI.GetComponent<AgentUI>();
        agentUI.canvas = agentUIParent.GetComponent<Canvas>();
        codeAgent.UI = agentUI;
    }

    public void SpawnEnemy(agentType type, Vector2 pos)
    {
        UIDataManager.Instance.TryCreateEnemyCodeParent(type);


        GameObject go = Instantiate(enemyPref, agentParent);
        CodeBlockAgent codeAgent = go.GetComponent<CodeBlockAgent>();
        codeAgent.startingCoords = pos;
        codeAgent.team = agentTeam.Enemy;
        codeAgent.type = type;


        GameObject UI = Instantiate(enemyUIPref, agentUIParent);
        AgentUI agentUI = UI.GetComponent<AgentUI>();
        agentUI.canvas = agentUIParent.GetComponent<Canvas>();
        codeAgent.UI = agentUI;
    }
}
