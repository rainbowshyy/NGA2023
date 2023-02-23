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

    public void AddAgent(agentType type)
    {
        Vector2Int pos = GridManager.Instance.GetRandomTileInRect(0, 0, 6, 1);
        playerAgents.Add(new playerAgentData(type, pos));
    }
}
