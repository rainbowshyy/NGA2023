using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRayBlock : CodeBlock
{
    public DamageRayBlock(int[] param) : base(param) { }
    private Vector2Int pointHit;

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        Vector2Int currentPos = agent.gridCoords;
        CodeBlockAgent hit = null;
        while (currentPos.x >= 0 && currentPos.x <= 6 && currentPos.y >= 0 && currentPos.y <= 6)
        {
            currentPos += new Vector2Int(parameters[1], parameters[2]);
            hit = GridManager.Instance.GetAgentAtCoords(currentPos.x, currentPos.y);
            if (hit != null)
            {
                hit.AddHealth(-parameters[0]);
                pointHit = currentPos;
                Debug.Log(pointHit);
                return true;
            }
        }
        pointHit = currentPos;
        Debug.Log(pointHit);
        return false;
    }

    public override string ShowSyntax()
    {
        int spriteIndex = 8;
        if (parameters[1] == 0)
        {
            if (parameters[2] < 0)
            {
                spriteIndex = 3;
            }
            else if (parameters[2] > 0)
            {
                spriteIndex = 2;
            }
        }
        else if (parameters[2] == 0)
        {
            if (parameters[1] < 0)
            {
                spriteIndex = 1;
            }
            else if (parameters[1] > 0)
            {
                spriteIndex = 0;
            }
        }
        else
        {
            if (parameters[1] > 0)
            {
                if (parameters[2] > 0)
                {
                    spriteIndex = 5;
                }
                else if (parameters[2] < 0)
                {
                    spriteIndex = 6;
                }
            }
            else if (parameters[1] < 0)
            {
                if (parameters[2] > 0)
                {
                    spriteIndex = 4;
                }
                else if (parameters[2] < 0)
                {
                    spriteIndex = 7;
                }
            }
        }
        return "<color=#ee5644>health</color> <sprite index=8> -= " + parameters[0] + " at Raycast ( <color=#4eb2f3>" + parameters[1] + "</color> , <color=#f38f4e>" + parameters[2] + "</color> ) <sprite index=" + spriteIndex + ">";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.damageLaser, agent.gridCoords, parameters, pointHit);
    }
}