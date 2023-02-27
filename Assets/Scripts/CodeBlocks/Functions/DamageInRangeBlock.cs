using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInRangeBlock : CodeBlock
{
    public DamageInRangeBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        foreach (CodeBlockAgent c in GridManager.Instance.GetAgentsInRange(agent.gridCoords.x, agent.gridCoords.y, parameters[0], false))
        {
            c.AddHealth(-agent.energy);
        }
        agent.energy = 0;
        return true;
    }

    public override string ShowSyntax()
    {
        return "Range( " + parameters[0] + " ) . <color=#ee5644>HP</color><sprite index=8>  -=  <color=#ebca54>PWR</color><sprite index=9>\n<color=#ebca54>PWR</color><sprite index=9>  =  0";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.damageRange, agent.gridCoords, parameters, Vector2Int.zero);
    }
}