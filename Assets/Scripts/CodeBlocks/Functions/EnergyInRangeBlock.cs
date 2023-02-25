using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyInRange : CodeBlock
{
    public EnergyInRange(int[] param) : base(param) { }

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
        return "<color=#ebca54>energy</color> <sprite index=9> += " + parameters[0] + " in Range ( " + parameters[1] + " )";
    }
}