using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPWRBlock : CodeBlock
{
    public HealthPWRBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        agent.AddHealth(agent.energy);
        agent.energy = 0;
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ee5644>HP</color><sprite index=8>  <color=#98f460>+=</color>  <color=#ebca54>PWR</color><sprite index=9>\n<color=#ebca54>PWR</color><sprite index=9>  =  0";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        //EffectManager.Instance.CreateEffect(EffectTypes.energyUp, agent.gridCoords, parameters, Vector2Int.zero);
    }

    public override string ToolTip()
    {
        return "This unit <color=#98f460>adds</color> its <color=#ebca54>PWR</color><sprite index=9> to its <color=#ee5644>HP</color><sprite index=8>. Then set this unit's <color=#ebca54>PWR</color><sprite index=9> to 0.";
    }
}