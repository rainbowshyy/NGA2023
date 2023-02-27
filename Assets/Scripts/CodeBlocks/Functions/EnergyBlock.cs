using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBlock : CodeBlock
{
    public EnergyBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        agent.AddEnergy(parameters[0]);
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ebca54>energy</color> <sprite index=9> += " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.energyUp, agent.gridCoords, parameters, Vector2Int.zero);
    }
}