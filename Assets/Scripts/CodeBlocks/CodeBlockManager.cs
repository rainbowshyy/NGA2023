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
            switch (s.code)
            {
                case CodeBlockTypes.MoveBlock:
                    codeBlocksReturn.Add(new MoveBlock(s.parameters));
                    break;
                case CodeBlockTypes.EnergyBlock:
                    codeBlocksReturn.Add(new EnergyBlock(s.parameters));
                    break;
                case CodeBlockTypes.WaitBlock:
                    codeBlocksReturn.Add(new WaitBlock(s.parameters));
                    break;
                case CodeBlockTypes.EnergyGreaterThan:
                    codeBlocksReturn.Add(new EnergyGreaterBlock(s.parameters));
                    break;
                case CodeBlockTypes.EnergyInRange:
                    codeBlocksReturn.Add(new EnergyInRange(s.parameters));
                    break;
                case CodeBlockTypes.DamageInRange:
                    codeBlocksReturn.Add(new DamageInRange(s.parameters));
                    break;
            }
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
            case CodeBlockTypes.EnergyInRange:
                codeBlocksReturn = new EnergyInRange(codeBlocksParam.parameters);
                break;
            case CodeBlockTypes.DamageInRange:
                codeBlocksReturn = new DamageInRange(codeBlocksParam.parameters);
                break;
        }
        return codeBlocksReturn;
    }
}
