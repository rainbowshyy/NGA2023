using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action onRoundLose;
    public static Action onRoundWin;
    public static Action onNewRound;

    private Stages currentStage;

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
    }

    private void OnEnable()
    {
        onRoundLose += LoseRound;
        onNewRound += NewRound;
    }

    private void OnDisable()
    {
        onRoundLose -= LoseRound;
        onNewRound -= NewRound;
    }

    private void StartGame()
    {
        currentStage = Stages.Intro;

        PlayerDataManager.Instance.AddAgent(agentType.Blue);

        SpawningManager.Instance.SpawnPlayerAgents();

        SpawningManager.SpawnCodeBlock(new CodeBlockStruct(CodeBlockTypes.MoveBlock, new int[2] { 0, 1 }, null));
        SpawningManager.SpawnCodeBlock(new CodeBlockStruct(CodeBlockTypes.DamageInRange, new int[2] { 1, 1 }, null));

        EncounterManager.Instance.PopulatePool(currentStage);
        EncounterManager.Instance.NextInPool();
    }

    private void NewRound()
    {
        GridManager.Instance.ResetObjects();
        EncounterManager.Instance.NextInPool();
    }

    private void LoseRound()
    {
        StartCoroutine(DelayLoadScene());
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(0);
    }
}
