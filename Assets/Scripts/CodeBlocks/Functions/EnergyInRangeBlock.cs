using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyInRangeBlock : CodeBlock
{
    public EnergyInRangeBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        foreach(CodeBlockAgent c in GridManager.Instance.GetAgentsInRange(agent.gridCoords.x, agent.gridCoords.y, parameters[1], false))
        {
            c.AddEnergy(parameters[0]);
        }
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ebca54>PWR</color><sprite index=9> in Range( " + parameters[1] + " )  <color=#98f460>+=</color>  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.powerRange, agent.gridCoords, parameters, Vector2Int.zero, true);
    }
    public override string ToolTip()
    {
        return "This unit <color=#98f460>adds</color> " + parameters[0] + " to other units' <color=#ebca54>PWR</color><sprite index=9> within " + parameters[1] + " tiles.";
    }
}