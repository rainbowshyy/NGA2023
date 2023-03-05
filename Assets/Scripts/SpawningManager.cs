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

    private int enemyCount;
    private int playerCount;

    public int EnemyCount { get { return enemyCount; } }
    public int PlayerCount { get { return playerCount; } }

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

    public void SpawnPlayerAgents()
    {
        foreach (playerAgentData p in PlayerDataManager.Instance.playerAgents)
        {
            SpawnCodeAgent(p.type, p.pos);
        }
    }

    public void SpawnCodeAgent(agentType type, Vector2Int pos)
    {
        UIDataManager.Instance.TryCreateCodeParent(type);

        GameObject go = Instantiate(agentPref, agentParent);
        CodeBlockAgent codeAgent = go.GetComponent<CodeBlockAgent>();
        codeAgent.startingCoords = pos;
        codeAgent.type = type;

        go.GetComponent<Animator>().runtimeAnimatorController = AgentManager.Instance.AgentAnimatorMap[type];

        GameObject UI = Instantiate(agentUIPref, agentUIParent);
        AgentUI agentUI = UI.GetComponent<AgentUI>();
        agentUI.canvas = agentUIParent.GetComponent<Canvas>();
        codeAgent.UI = agentUI;

        ChangePlayerCount(1);

        codeAgent.Setup();
    }

    public void SpawnEnemy(agentType type, Vector2Int pos)
    {
        UIDataManager.Instance.TryCreateEnemyCodeParent(type);


        GameObject go = Instantiate(enemyPref, agentParent);
        CodeBlockAgent codeAgent = go.GetComponent<CodeBlockAgent>();
        codeAgent.startingCoords = pos;
        codeAgent.team = agentTeam.Enemy;
        codeAgent.type = type;

        go.GetComponent<Animator>().runtimeAnimatorController = AgentManager.Instance.AgentAnimatorMap[type];

        GameObject UI = Instantiate(enemyUIPref, agentUIParent);
        AgentUI agentUI = UI.GetComponent<AgentUI>();
        agentUI.canvas = agentUIParent.GetComponent<Canvas>();
        codeAgent.UI = agentUI;

        ChangeEnemyCount(1);
        
        codeAgent.Setup();
    }

    public void ChangeEnemyCount(int change)
    {
        enemyCount += change;
        if (enemyCount <= 0) 
        {
            GameManager.onRoundEnd?.Invoke(true);
        }
    }

    public void ChangePlayerCount(int change)
    {
        playerCount += change;
        if (playerCount <= 0)
        {
            DoDamage();
        }
    }

    public void ResetCount()
    {
        enemyCount = 0;
        playerCount = 0;
    }

    public void DoDamage()
    {
        GameManager.onTakeDamage?.Invoke(enemyCount);
        enemyCount = 0;
        playerCount = 0;
    }

    public static void SpawnCodeBlock(CodeBlockStruct code)
    {
        UIDataManager.Instance.CreateCodeBlock(CodeBlockManager.GetCodeFromStruct(code));
    }
}