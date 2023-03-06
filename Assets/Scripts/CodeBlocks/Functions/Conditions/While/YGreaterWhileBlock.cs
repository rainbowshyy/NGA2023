using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YGreaterWhileBlock : WhileBlock
{
    public YGreaterWhileBlock(int[] param) : base(param) { conditionType = ConditionOptionType.Int; }

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
        return "While <color=#f38f4e><b> Y</b></color>  >  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.coordCheck, agent.gridCoords, new int[2] { 1, 0 }, Vector2Int.zero, agent.gridCoords.y > parameters[0]);
    }
    public override string ToolTip()
    {
        return "Continuously runs this block's nested codeblocks while this unit's<color=#f38f4e><b> Y</b></color> position is greater than " + parameters[0] + ".";
    }
}