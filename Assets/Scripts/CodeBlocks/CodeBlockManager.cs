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

        codeBlocks.Add(agentType.Blue, new List<CodeBlock>());
    }

    private void OnEnable()
    {
        GameManager.onRoundLose += StopProgram;
        GameManager.onRoundWin += StopProgram;
    }

    private void OnDisable()
    {
        GameManager.onRoundLose -= StopProgram;
        GameManager.onRoundWin -= StopProgram;
    }

    public void StartProgram()
    {
        step = 0;
        onStartProgram?.Invoke();
        codeCo = StartCoroutine(CodeStep());
        InputManager.onToggleInputs?.Invoke(false);
    }

    public void StopProgram()
    {
        StopCoroutine(codeCo);
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
        Debug.Log("new speed: " + speedMult);
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
        yield return new WaitForSeconds(speedMult);
        onDoCode?.Invoke(agentTeam.Player, step);
        yield return new WaitForSeconds(speedMult);
        onDoCode?.Invoke(agentTeam.Enemy, step);
        step += 1;
        if (step < 36)
            codeCo = StartCoroutine(CodeStep());
        else
            GameManager.onRoundLose?.Invoke();
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
                    toAdd = new EnergyInRange(s.parameters);
                    break;
                case CodeBlockTypes.DamageInRange:
                    toAdd = new DamageInRange(s.parameters);
                    break;
            }
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
                codeBlocksReturn = new EnergyInRange(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.DamageInRange:
                codeBlocksReturn = new DamageInRange(codeBlocksParam.parameters);
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
        }
        return codeBlocksReturn;
    }
}
