using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnergyRangePowerBlock : CodeBlock
{
    public EnergyRangePowerBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        agent.AddEnergy(1);
        foreach(CodeBlockAgent c in GridManager.Instance.GetAgentsInRange(agent.gridCoords.x, agent.gridCoords.y, parameters[1], false))
        {
            c.AddEnergy(agent.energy);
        }
        agent.energy = 0;
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ebca54>PWR</color><sprite index=9>  <color=#98f460>+=</color>  1" +
            "\n<color=#ebca54>PWR</color><sprite index=9> in Range( " + parameters[1] + " )  <color=#98f460>+=</color>  <color=#ebca54>PWR</color><sprite index=9>" +
            "\n<color=#ebca54>PWR</color><sprite index=9>  =  0";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.powerRange, agent.gridCoords, parameters, Vector2Int.zero, true);
    }
    public override string ToolTip()
    {
        return "This unit <color=#98f460>adds</color> 1 to its <color=#ebca54>PWR</color><sprite index=9>. Then this unit <color=#98f460>adds</color> its <color=#ebca54>PWR</color> to other units' <color=#ebca54>PWR</color><sprite index=9> within " + parameters[1] + " tiles. Then set this unit's <color=#ebca54>PWR</color><sprite index=9> to 0.";
    }
}