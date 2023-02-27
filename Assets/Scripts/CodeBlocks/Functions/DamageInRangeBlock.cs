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
        foreach (CodeBlockAgent c in GridManager.Instance.GetAgentsInRange(agent.gridCoords.x, agent.gridCoords.y, parameters[1], false))
        {
            c.AddHealth(-parameters[0]);
        }
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ee5644>health</color> <sprite index=8> -= " + parameters[0] + " in Range ( " + parameters[1] + " )";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.damageRange, agent.gridCoords, parameters, Vector2Int.zero);
    }
}