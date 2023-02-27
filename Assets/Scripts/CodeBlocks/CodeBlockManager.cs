using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBlockManager : MonoBehaviour
{
    public static System.Action<agentTeam, int> onDoCode;
    public static System.Action onStartProgram;

    public Dictionary<agentType, List<CodeBlock>> codeBlocks;

    private int step;
    private float speedMult = 0.5f;
    private bool stopped = false;

    Coroutine codeCo;

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
    }

    private void OnDisable()
    {
        GameManager.onRoundEnd -= StopProgram;
    }

    public void StartProgram()
    {
        step = 0;
        stopped = false;
        onStartProgram?.Invoke();
        codeCo = StartCoroutine(CodeStep());
        InputManager.onToggleInputs?.Invoke(false);
    }

    public void StopProgram(bool win)
    {
        stopped = true;
        StopCoroutine(codeCo);
        Debug.Log("yep");
        InputManager.onToggleInputs?.Invoke(true);
    }

    public void ChangeSpeed(bool positive)
    {
        if (positive && speedMult > 0.0625f)
        {
            speedMult *= 0.5f;
        }
        else if (!positive && speedMult < 2)
        {
            speedMult *= 2f;
        }
    }

    public int GetStepForType(agentType type, int totalStep)
    {
        return totalStep % codeBlocks[type].Count;
    }

    public bool TypeHasCode(agentType type)
    {
        return (codeBlocks[type].Count > 0);
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
            codeCo = StartCoroutine(CodeStep());
        else
            SpawningManager.Instance.DoDamage();
    }

    public void SetCodeForType(agentType type, List<CodeBlock> codeBlocksParam)
    {
        codeBlocks[type] = codeBlocksParam;
    }

    public static List<CodeBlock> GetCodeListFromStruct(List<CodeBlockStruct> codeBlocksParam)
    {
        List<CodeBlock> codeBlocksReturn = new List<CodeBlock>();

        foreach (CodeBlockStruct s in codeBlocksParam)
        {
            CodeBlock toAdd = new WaitBlock(s.parameters);
            /*
            switch (s.code)
            {
                case CodeBlockTypes.MoveBlock:
                    toAdd = new MoveBlock(s.parameters);
                    break;
                case CodeBlockTypes.EnergyBlock:
                    toAdd = new EnergyBlock(s.parameters);
                    break;
                case CodeBlockTypes.WaitBlock:
                    toAdd = new WaitBlock(s.parameters);
                    break;
                case CodeBlockTypes.EnergyGreaterThan:
                    toAdd = new EnergyGreaterBlock(s.parameters);
                    break;
                case CodeBlockTypes.EnergyInRange:
                    toAdd = new EnergyInRangeBlock(s.parameters);
                    break;
                case CodeBlockTypes.DamageInRange:
                    toAdd = new DamageInRange(s.parameters);
                    break;
            }
            */

            toAdd = GetCodeFromStruct(s);

            if (s.scope != null && s.scope.Count > 0)
            {
                toAdd.scope = GetCodeListFromStruct(s.scope);
            }
            codeBlocksReturn.Add(toAdd);
        }

        return codeBlocksReturn;
    }

    public static CodeBlock GetCodeFromStruct(CodeBlockStruct codeBlocksParam)
    {
        CodeBlock codeBlocksReturn = new WaitBlock(codeBlocksParam.parameters);

        switch (codeBlocksParam.code)
        {
            case CodeBlockTypes.MoveBlock:
                codeBlocksReturn = new MoveBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnergyBlock:
                codeBlocksReturn = new EnergyBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.WaitBlock:
                codeBlocksReturn = new WaitBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnergyGreaterThan:
                codeBlocksReturn = new EnergyGreaterBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnergyLessThan:
                codeBlocksReturn = new EnergyLessBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnergyInRange:
                codeBlocksReturn = new EnergyInRangeBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.DamageInRange:
                codeBlocksReturn = new DamageInRangeBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.XGreaterThan:
                codeBlocksReturn = new XGreaterBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.XLessThan:
                codeBlocksReturn = new XLessBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.YGreaterThan:
                codeBlocksReturn = new YGreaterBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.YLessThan:
                codeBlocksReturn = new YLessBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnergyGreaterWhile:
                codeBlocksReturn = new EnergyGreaterWhileBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnergyLessWhile:
                codeBlocksReturn = new EnergyLessWhileBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.XGreaterWhile:
                codeBlocksReturn = new XGreaterWhileBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.XLessWhile:
                codeBlocksReturn = new XLessWhileBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.YGreaterWhile:
                codeBlocksReturn = new YGreaterWhileBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.YLessWhile:
                codeBlocksReturn = new YLessWhileBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.Space:
                codeBlocksReturn = new SpaceBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.SpaceWhile:
                codeBlocksReturn = new SpaceWhileBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.DamageRay:
                codeBlocksReturn = new DamageRayBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnemyRay:
                codeBlocksReturn = new EnemyRayBlock(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.EnemyRayWhile:
                codeBlocksReturn = new EnemyRayWhileBlock(codeBlocksParam.parameters);
                break;
        }
        return codeBlocksReturn;
    }
}
