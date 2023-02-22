using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeBlockTypes { MoveBlock, EnergyBlock, WaitBlock, EnergyInRange, EnergyGreaterThan, DamageInRange}

public abstract class CodeBlock
{
    public int[] parameters;
    public bool isCondition;
    public List<CodeBlock> scope;

    protected CodeBlock(int[] param)
    {
        parameters = param;
        isCondition = false;
    }

    public abstract bool ReadyCode(CodeBlockAgent agent);
    public abstract bool RunCode(CodeBlockAgent agent);
    public abstract string ShowSyntax();
}
