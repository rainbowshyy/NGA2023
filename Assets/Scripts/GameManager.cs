using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<int> onTakeDamage;
    public static Action onNewRound;
    public static Action onNewRoundStarted;
    public static Action<bool> onRoundEnd;
    public static Action<Stages> onNewStage;

    public static Action<int> onNewGold;
    public static Action<int> onNewHealth;

    public Stages currentStage;
    private int health;
    private int gold;

    [SerializeField] private int[] stageEncounterCount;

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
        onTakeDamage += TakeDamage;
        onNewRound += NewRound;
        onNewStage += NewStage;
    }

    private void OnDisable()
    {
        onTakeDamage -= TakeDamage;
        onNewRound -= NewRound;
        onNewStage -= NewStage;
    }

    private void StartGame()
    {
        currentStage = Stages.Intro;
        health = 10;
        gold = 5;
        onNewHealth?.Invoke(health);

        PlayerDataManager.Instance.AddAgent(agentType.Cubert, false);

        SpawningManager.SpawnCodeBlock(new CodeBlockStruct(CodeBlockTypes.MoveBlock, new int[2] { 0, 1 }, null, 2));
        SpawningManager.SpawnCodeBlock(new CodeBlockStruct(CodeBlockTypes.DamageInRange, new int[2] { 1, 1 }, null, 4));
        SpawningManager.SpawnCodeBlock(new CodeBlockStruct(CodeBlockTypes.EnergyBlock, new int[2] { 1, 0 }, null, 4));

        EncounterManager.Instance.PopulatePool(stageEncounterCount);
        onNewRound?.Invoke();
    }

    private void NewRound()
    {
        InputManager.onToggleInputs?.Invoke(true);
        UIDataManager.Instance.RemoveCodeParents();
        GridManager.Instance.ResetObjects();
        EncounterManager.Instance.NextInPool();
        SpawningManager.Instance.SpawnPlayerAgents();
        onNewRoundStarted?.Invoke();
    }

    private void TakeDamage(int loss)
    {
        onRoundEnd?.Invoke(false);
        health -= loss;
        onNewHealth?.Invoke(health);
        if (health <= 0)
        {
            StartCoroutine(DelayLoadScene());
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        onNewGold?.Invoke(gold);
    }

    public bool EnoughGold(int amount)
    {
        return gold >= amount;
    }

    private void NewStage(Stages stage)
    {
        currentStage = stage;
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(0);
    }
}
