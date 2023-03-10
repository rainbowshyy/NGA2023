using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Color[] actColor;
    
    public static Action<int> onTakeDamage;
    public static Action onNewRound;
    public static Action onNewRoundStarted;
    public static Action<bool> onRoundEnd;
    public static Action<Stages> onNewStage;

    public static Action<int> onNewGold;
    public static Action<int> onNewHealth;

    public Stages currentStage;
    public int currentRound;
    private int health;
    private int gold;

    [SerializeField] private int enemyPowerIncreaseInterval;
    [SerializeField] private int enemyHealthIncreaseInterval;
    private int nextEnemyHealthIncrease;
    private int nextEnemyEnergyIncrease;
    [SerializeField] private int[] stageEncounterCount;

    public int EnemyHealthMod { get; private set; }
    public int EnemyPowerMod { get; private set; }

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
        currentRound = 0;
        currentStage = Stages.Intro;
        health = 10;
        gold = 5;
        onNewHealth?.Invoke(health);

        EnemyHealthMod = 1;
        EnemyPowerMod = 1;
        nextEnemyEnergyIncrease = enemyPowerIncreaseInterval;
        nextEnemyHealthIncrease = enemyHealthIncreaseInterval;

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
        SpawningManager.Instance.SpawnPlayerAgents();
        if (!EncounterManager.Instance.NextInPool())
        {
            DelayLoadScene(2);
        }
        onNewRoundStarted?.Invoke();
        currentRound += 1;
        if (currentRound > 1)
        {
            AddGold(1);
        }
        /*
        if (currentRound >= nextEnemyHealthIncrease)
        {
            EnemyHealthMod += 1;
            nextEnemyHealthIncrease += enemyHealthIncreaseInterval;
        }
        if (currentRound >= nextEnemyEnergyIncrease)
        {
            EnemyPowerMod += 1;
            nextEnemyEnergyIncrease += enemyPowerIncreaseInterval;
        }*/
    }

    private void TakeDamage(int loss)
    {
        onRoundEnd?.Invoke(false);
        health -= loss;
        onNewHealth?.Invoke(health);
        if (health <= 0)
        {
            StartCoroutine(DelayLoadScene(3));
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
        if (stage != Stages.Intro && stage != Stages.Intro2 && stage != Stages.Act1)
        {
            EnemyPowerMod += 1;
            EnemyHealthMod += 1;
        }
    }

    IEnumerator DelayLoadScene(int id)
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(id);
    }
}
