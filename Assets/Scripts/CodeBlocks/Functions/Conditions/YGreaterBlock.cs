using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YGreaterBlock : ConditionBlock
{
    public YGreaterBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return agent.gridCoords.y > parameters[0];
    }

    public override string ShowSyntax()
    {
        return "If <color=#f38f4e><b> Y</b></color>  >  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.coordCheck, agent.gridCoords, new int[2] { 1, 0 }, Vector2Int.zero, agent.gridCoords.y > parameters[0]);
    }
    public override string ToolTip()
    {
        return "Runs this block's nested codeblocks if this unit's<color=#f38f4e><b> Y</b></color> position is greater than " + parameters[0] + ".";
    }
}