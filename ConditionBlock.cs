using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionBlock : CodeBlock
{
    public ConditionBlock(int[] param) : base(param) { isCondition = true; scope = new List<CodeBlock>(); }

    public void AddBlock(CodeBlock block)
    {
        scope.Add(block);
    }
}