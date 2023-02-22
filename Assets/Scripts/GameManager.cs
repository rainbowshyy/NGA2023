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
        SpawningManager.Instance.SpawnCodeAgent(agentType.Blue, new Vector2(3, 0));
        SpawningManager.Instance.SpawnEnemy(agentType.Blob, new Vector2(3, 6));
    }
}
