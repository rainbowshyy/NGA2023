using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInRangeHealthBlock : CodeBlock
{
    public DamageInRangeHealthBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        foreach (CodeBlockAgent c in GridManager.Instance.GetAgentsInRange(agent.gridCoords.x, agent.gridCoords.y, parameters[0], false))
        {
            c.AddHealth(-agent.health * 2);
        }

        AudioManager.onAudioEvent?.Invoke(audioEvent.Damage, agent.energy);

        agent.health = 1;
        return true;
    }

    public override string ShowSyntax()
    {
        return "<color=#ee5644>HP</color><sprite index=8> in Range( " + parameters[0] + " )  <color=#f3574e>-=</color>  <color=#ee5644>HP</color><sprite index=8>\n<color=#ee5644>HP</color><sprite index=8>  =  1";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.damageRange, agent.gridCoords, parameters, Vector2Int.zero, true);
    }
    public override string ToolTip()
    {
        return "This unit <color=#f3574e>removes</color> <color=#ee5644>HP</color><sprite index=8> equal to this unit's <color=#ee5644>HP</color><sprite index=8> from other units within " + parameters[0] + " tiles. Then set this unit's <color=#ee5644>HP</color><sprite index=8> to 1.";
    }
}