using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGreaterBlock : ConditionBlock
{
    public HealthGreaterBlock(int[] param) : base(param) { conditionType = ConditionOptionType.Int; }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return agent.health > parameters[0];
    }

    public override string ShowSyntax()
    {
        return "If <color=#ee5644>HP</color><sprite index=8>  >  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }
    public override string ToolTip()
    {
        return "Runs this block's nested codeblocks if this unit's <color=#ee5644>HP</color><sprite index=8> is greater than " + parameters[0] + ".";
    }
}