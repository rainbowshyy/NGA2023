using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyLessBlock : ConditionBlock
{
    public EnergyLessBlock(int[] param) : base(param) { }

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
        return "If <color=#ebca54>PWR</color><sprite index=9>  <  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }
}