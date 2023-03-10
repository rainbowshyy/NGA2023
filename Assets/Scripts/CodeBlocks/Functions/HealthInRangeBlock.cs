using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthInRangeBlock : CodeBlock
{
    public HealthInRangeBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        foreach (CodeBlockAgent c in GridManager.Instance.GetAgentsInRange(agent.gridCoords.x, agent.gridCoords.y, parameters[0], false))
        {
            c.AddHealth(agent.energy);
        }
        agent.energy = 0;
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ee5644>HP</color><sprite index=8> in Range( " + parameters[0] + " )  <color=#98f460>+=</color>  <color=#ebca54>PWR</color><sprite index=9>\n<color=#ebca54>PWR</color><sprite index=9>  =  0";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.healthRange, agent.gridCoords, parameters, Vector2Int.zero, true);
    }
    public override string ToolTip()
    {
        return "This unit <color=#98f460>adds</color> <color=#ee5644>HP</color><sprite index=8> equal to this unit's <color=#ebca54>PWR</color><sprite index=9> from other units within " + parameters[0] + " tiles. Then set this unit's <color=#ebca54>PWR</color><sprite index=9> to 0.";
    }
}