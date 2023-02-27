using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct playerAgentData
{
    public agentType type;
    public Vector2Int pos;
    public playerAgentData(agentType type, Vector2Int pos)
    {
        this.type = type;
        this.pos = pos;
    }
}

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField] private List<agentType> possibleAgents;

    public List<playerAgentData> playerAgents;

    public static PlayerDataManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerAgents = new List<playerAgentData>();
    }

    private void OnEnable()
    {
        GameManager.onNewStage += AddNewAgent;
    }

    private void OnDisable()
    {
        GameManager.onNewStage -= AddNewAgent;
    }

    public void AddAgent(agentType type)
    {
        Vector2Int pos = GridManager.Instance.GetRandomTileInRect(0, 0, 6, 1);
        playerAgents.Add(new playerAgentData(type, pos));
    }

    public void SetAgent(agentType type, Vector2Int pos)
    {
        playerAgents.Add(new playerAgentData(type, pos));
    }

    private void AddNewAgent(Stages stage)
    {
        if (stage == Stages.Act1)
        {
            AddAgent(agentType.SirKel);
        }
        if (stage == Stages.Act2)
        {
            AddAgent(agentType.Treasure);
        }    
    }
}
