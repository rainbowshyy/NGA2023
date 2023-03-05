using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBlock : CodeBlock
{
    public HealthBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        agent.AddHealth(parameters[0]);
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ee5644>HP</color><sprite index=8>  <color=#98f460>+=</color>  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        //EffectManager.Instance.CreateEffect(EffectTypes.energyUp, agent.gridCoords, parameters, Vector2Int.zero);
    }

    public override string ToolTip()
    {
        return "This unit <color=#98f460>adds</color> " + parameters[0] + " to its <color=#ee5644>HP</color><sprite index=8>.";
    }
}