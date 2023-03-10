using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyLessWhileBlock : WhileBlock
{
    public EnergyLessWhileBlock(int[] param) : base(param) { conditionType = ConditionOptionType.Int; }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return agent.energy < parameters[0];
    }

    public override string ShowSyntax()
    {
        return "While <color=#ebca54>PWR</color><sprite index=9>  <  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }
    public override string ToolTip()
    {
        return "Continuously runs this block's nested codeblocks while this unit's <color=#ebca54>PWR</color><sprite index=9> is less than " + parameters[0] + ".";
    }
}