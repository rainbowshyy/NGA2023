using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XGreaterWhileBlock : WhileBlock
{
    public XGreaterWhileBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return agent.gridCoords.x > parameters[0];
    }

    public override string ShowSyntax()
    {
        return "While <color=#4eb2f3><b> X</b></color>  >  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.coordCheck, agent.gridCoords, new int[2] { 0, -1 }, Vector2Int.zero, agent.gridCoords.x > parameters[0]);
    }

    public override string ToolTip()
    {
        return "Continuously runs this block's nested codeblocks while this unit's<color=#4eb2f3><b> X</b></color> position is greater than " + parameters[0] + ".";
    }
}