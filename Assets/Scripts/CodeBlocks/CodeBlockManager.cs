using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CodeBlockManager : MonoBehaviour
{
    public static System.Action<agentTeam, int> onDoCode;
    public static System.Action onStartProgram;

    public Dictionary<agentType, List<CodeBlock>> codeBlocks;

    private int step;
    private float speedMult = 0.5f;
    private bool stopped = false;
    private bool lastEnemy = true;

    private int[] order = new int[2];

    public static CodeBlockManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        codeBlocks = new Dictionary<agentType, List<CodeBlock>>();

        codeBlocks.Add(agentType.Cubert, new List<CodeBlock>());
    }

    private void OnEnable()
    {
        GameManager.onRoundEnd += StopProgram;
        AudioManager.onBeat += CodeStepAudio;
    }

    private void OnDisable()
    {
        GameManager.onRoundEnd -= StopProgram;
        AudioManager.onBeat -= CodeStepAudio;
    }

    public int GetOrder(bool add, agentTeam team)
    {
        int index = 0;
        if (team == agentTeam.Enemy)
        {
            index = 1;
        }
        if (add)
        {
            order[index] += 1;
        }
        else
        {
            order[index] -= 1;
        }
        return order[index];
    }

    public void StartProgram()
    {
        lastEnemy = true;
        step = 0;
        stopped = false;
        //codeCo = StartCoroutine(CodeStep());
        InputManager.onToggleInputs?.Invoke(false);
        onStartProgram?.Invoke();
    }

    public void StopProgram(bool win)
    {
        stopped = true;
        //StopCoroutine(codeCo);
    }

    public bool TypeHasCode(agentType type)
    {
        return (codeBlocks[type].Count > 0);
    }

    private void CodeStepAudio()
    {
        if (lastEnemy)
        {
            lastEnemy = false;
            onDoCode?.Invoke(agentTeam.Player, step);
        }
        else
        {
            lastEnemy = true;
            onDoCode?.Invoke(agentTeam.Enemy, step);
            step += 1;
        }
    }

    IEnumerator CodeStep()
    {
        if (stopped)
        {
            yield break;
        }
        yield return new WaitForSeconds(speedMult);
        onDoCode?.Invoke(agentTeam.Player, step);
        if (stopped)
        {
            yield break;
        }
        yield return new WaitForSeconds(speedMult);
        onDoCode?.Invoke(agentTeam.Enemy, step);
        step += 1;
        if (stopped)
        {
            yield break;
        }
        if (step < 36)
            yield break;
        //codeCo = StartCoroutine(CodeStep());
        else
            SpawningManager.Instance.DoDamage();
    }

    public void SetCodeForType(agentType type, List<CodeBlock> codeBlocksParam)
    {
        codeBlocks[type] = codeBlocksParam;
        switch (type)
        {
            case agentType.Cubert:
                AudioManager.onAudioEvent?.Invoke(audioEvent.CubertIntensity, codeBlocksParam.Count);
                break;
            case agentType.SirKel:
                AudioManager.onAudioEvent?.Invoke(audioEvent.SirKelIntensity, codeBlocksParam.Count);
                break;
            case agentType.Treasure:
                AudioManager.onAudioEvent?.Invoke(audioEvent.TreasureIntensity, codeBlocksParam.Count);
                break;
        }
    }

    public static List<CodeBlock> GetCodeListFromStruct(List<CodeBlockStruct> codeBlocksParam, bool energyScaling)
    {
        List<CodeBlock> codeBlocksReturn = new List<CodeBlock>();

        foreach (CodeBlockStruct s in codeBlocksParam)
        {
            CodeBlock toAdd = new WaitBlock(s.Parameters);

            toAdd = GetCodeFromStruct(s, energyScaling);

            if (s.scope != null && s.scope.Count > 0)
            {
                toAdd.scope = GetCodeListFromStruct(s.scope, energyScaling);
            }
            codeBlocksReturn.Add(toAdd);
        }

        return codeBlocksReturn;
    }

    public static CodeBlock GetCodeFromStruct(CodeBlockStruct codeBlocksParam, bool energyScaling)
    {
        CodeBlock codeBlocksReturn = new WaitBlock(codeBlocksParam.Parameters);

        switch (codeBlocksParam.code)
        {
            case CodeBlockTypes.MoveBlock:
                codeBlocksReturn = new MoveBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyBlock:
                codeBlocksReturn = new EnergyBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.WaitBlock:
                codeBlocksReturn = new WaitBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyGreaterThan:
                codeBlocksReturn = new EnergyGreaterBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyLessThan:
                codeBlocksReturn = new EnergyLessBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyInRange:
                codeBlocksReturn = new EnergyInRangeBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.DamageInRange:
                codeBlocksReturn = new DamageInRangeBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.XGreaterThan:
                codeBlocksReturn = new XGreaterBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.XLessThan:
                codeBlocksReturn = new XLessBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.YGreaterThan:
                codeBlocksReturn = new YGreaterBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.YLessThan:
                codeBlocksReturn = new YLessBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyGreaterWhile:
                codeBlocksReturn = new EnergyGreaterWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyLessWhile:
                codeBlocksReturn = new EnergyLessWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.XGreaterWhile:
                codeBlocksReturn = new XGreaterWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.XLessWhile:
                codeBlocksReturn = new XLessWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.YGreaterWhile:
                codeBlocksReturn = new YGreaterWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.YLessWhile:
                codeBlocksReturn = new YLessWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.Space:
                codeBlocksReturn = new SpaceBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.SpaceWhile:
                codeBlocksReturn = new SpaceWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.DamageRay:
                codeBlocksReturn = new DamageRayBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnemyRay:
                codeBlocksReturn = new EnemyRayBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnemyRayWhile:
                codeBlocksReturn = new EnemyRayWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.HealthRange:
                codeBlocksReturn = new HealthInRangeBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.PushBlock:
                codeBlocksReturn = new PushBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergySetBlock:
                codeBlocksReturn = new EnergySetBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.Health:
                codeBlocksReturn = new HealthBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.HealthPWR:
                codeBlocksReturn = new HealthPWRBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.Create:
                codeBlocksReturn = new CreateBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyRay:
                codeBlocksReturn = new PowerRayBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.EnergyRayPower:
                codeBlocksReturn = new PowerRayPowerBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.PowerRangePower:
                codeBlocksReturn = new EnergyRangePowerBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.DamageRangeHealth:
                codeBlocksReturn = new DamageInRangeHealthBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.HealthGreater:
                codeBlocksReturn = new HealthGreaterBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.HealthGreaterWhile:
                codeBlocksReturn = new HealthGreaterWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.HealthLess:
                codeBlocksReturn = new HealthLessBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.HealthLessWhile:
                codeBlocksReturn = new HealthLessWhileBlock(codeBlocksParam.Parameters);
                break;
            case CodeBlockTypes.HealthRay:
                codeBlocksReturn = new HealthRayBlock(codeBlocksParam.Parameters);
                break;
        }

        codeBlocksReturn.price = codeBlocksParam.price;
        List<CodeBlockTypes> energyBlocks = new List<CodeBlockTypes>() { CodeBlockTypes.EnergyBlock, CodeBlockTypes.EnergyRay, CodeBlockTypes.EnergyInRange, CodeBlockTypes.EnergyGreaterThan,
        CodeBlockTypes.EnergyLessThan, CodeBlockTypes.EnergyLessWhile, CodeBlockTypes.EnergyGreaterWhile, CodeBlockTypes.EnergySetBlock};
        if (energyScaling && energyBlocks.Contains(codeBlocksParam.code))
        {
            int prev = codeBlocksReturn.parameters[0];
            codeBlocksReturn.parameters[0] *= GameManager.Instance.EnemyPowerMod;
            Debug.Log(prev + " , " + codeBlocksReturn.parameters[0]);
        }
        return codeBlocksReturn;
    }
}
