using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInRange : CodeBlock
{
    public DamageInRange(int[] param) : base(param) { }

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
        return "<color=#ee5644>health</color> <sprite index=8> -= " + parameters[0] + " to others in Range ( " + parameters[1] + " )";
    }
}