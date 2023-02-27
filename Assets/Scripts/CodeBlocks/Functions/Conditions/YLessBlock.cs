using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YLessBlock : ConditionBlock
{
    public YLessBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return agent.gridCoords.y < parameters[0];
    }

    public override string ShowSyntax()
    {
        return "If <color=#f38f4e><b> Y</b></color>  <  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }
}