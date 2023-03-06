using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XLessBlock : ConditionBlock
{
    public XLessBlock(int[] param) : base(param) { conditionType = ConditionOptionType.Int; }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return agent.gridCoords.x < parameters[0];
    }

    public override string ShowSyntax()
    {
        return "If <color=#4eb2f3><b> X</b></color>  <  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.coordCheck, agent.gridCoords, new int[2] { 0, -1 }, Vector2Int.zero, agent.gridCoords.x < parameters[0]);
    }
    public override string ToolTip()
    {
        return "Runs this block's nested codeblocks if this unit's<color=#4eb2f3><b> X</b></color> position is less than " + parameters[0] + ".";
    }
}