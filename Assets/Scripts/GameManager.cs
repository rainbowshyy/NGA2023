using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action onRoundLose;
    public static Action onRoundWin;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartGame();
        SpawningManager.Instance.SpawnEnemy(agentType.Blob, new Vector2Int(3, 6));
        onRoundLose += LoseRound;
    }

    private void StartGame()
    {
        PlayerDataManager.Instance.AddAgent(agentType.Blue);

        SpawningManager.Instance.SpawnPlayerAgents();

        SpawningManager.SpawnCodeBlock(new CodeBlockStruct(CodeBlockTypes.MoveBlock, new int[2] { 0, 1 }));
        SpawningManager.SpawnCodeBlock(new CodeBlockStruct(CodeBlockTypes.DamageInRange, new int[2] { 1, 1 }));
    }

    private void LoseRound()
    {
        SceneManager.LoadScene(0);
    }
}
