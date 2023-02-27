using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeBlockTypes { MoveBlock, EnergyBlock, WaitBlock, EnergyInRange, EnergyGreaterThan, EnergyLessThan, DamageInRange, XGreaterThan, XLessThan, YGreaterThan, YLessThan, EnergyGreaterWhile, EnergyLessWhile, XGreaterWhile, XLessWhile, YGreaterWhile, YLessWhile, Space, SpaceWhile, DamageRay, EnemyRay, EnemyRayWhile}

public abstract class CodeBlock
{
    public int[] parameters;
    public bool isCondition;
    public bool isLoop;
    public List<CodeBlock> scope;

    protected CodeBlock(int[] param)
    {
        parameters = param;
        isCondition = false;
        isLoop = false;
    }

    public abstract bool ReadyCode(CodeBlockAgent agent);
    public abstract bool RunCode(CodeBlockAgent agent);
    public abstract string ShowSyntax();
    public abstract void VisualCode(CodeBlockAgent agent);
    public abstract string ToolTip();
}
