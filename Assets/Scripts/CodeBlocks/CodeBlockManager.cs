using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBlockManager : MonoBehaviour
{
    public static System.Action<agentTeam, int> onDoCode;

    public Dictionary<agentType, List<CodeBlock>> codeBlocks;

    private int step;

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

        //codeBlocks.Add(agentType.Blue, new List<CodeBlock>() { new MoveBlock(new int[2] { 0, 1}) });
    }

    private void Start()
    {
        step = 0;
        StartCoroutine(CodeStep());
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
        yield return new WaitForSeconds(1f);
        onDoCode?.Invoke(agentTeam.Player, step);
        step += 1;
        StartCoroutine(CodeStep());
    }

    public void SetCodeForType(agentType type, List<CodeBlock> codeBlocksParam)
    {
        codeBlocks[type] = codeBlocksParam;
    }
}
