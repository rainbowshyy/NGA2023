using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeBlockTypes { MoveBlock, EnergyBlock, WaitBlock}

public abstract class CodeBlock
{
    public int[] parameters;

    protected CodeBlock(int[] param)
    {
        parameters = param;
    }

    public abstract bool ReadyCode(CodeBlockAgent agent);
    public abstract bool RunCode(CodeBlockAgent agent);
    public abstract string ShowSyntax();
}
