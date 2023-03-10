using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRayBlock : CodeBlock
{
    public PowerRayBlock(int[] param) : base(param) { }
    private Vector2Int pointHit;

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        Vector2Int currentPos = agent.gridCoords;
        GridElement hit = null;
        while (currentPos.x >= 0 && currentPos.x <= 6 && currentPos.y >= 0 && currentPos.y <= 6)
        {
            currentPos += new Vector2Int(parameters[0], parameters[1]);
            hit = GridManager.Instance.GetAgentAtCoords(currentPos.x, currentPos.y);
            if (hit != null)
            {
                if (hit is CodeBlockAgent)
                {
                    CodeBlockAgent agentHit = (CodeBlockAgent)hit;
                    agentHit.AddEnergy(parameters[2]);
                }
                pointHit = currentPos;
                return true;
            }
        }
        pointHit = currentPos;
        return false;
    }

    public override string ShowSyntax()
    {
        int spriteIndex = 8;
        if (parameters[0] == 0)
        {
            if (parameters[1] < 0)
            {
                spriteIndex = 3;
            }
            else if (parameters[1] > 0)
            {
                spriteIndex = 2;
            }
        }
        else if (parameters[1] == 0)
        {
            if (parameters[0] < 0)
            {
                spriteIndex = 1;
            }
            else if (parameters[0] > 0)
            {
                spriteIndex = 0;
            }
        }
        else
        {
            if (parameters[0] > 0)
            {
                if (parameters[1] > 0)
                {
                    spriteIndex = 5;
                }
                else if (parameters[1] < 0)
                {
                    spriteIndex = 6;
                }
            }
            else if (parameters[0] < 0)
            {
                if (parameters[1] > 0)
                {
                    spriteIndex = 4;
                }
                else if (parameters[1] < 0)
                {
                    spriteIndex = 7;
                }
            }
        }
        return "<color=#ebca54>PWR</color><sprite index=9> at Raycast( <color=#4eb2f3>" + parameters[0] + "</color> , <color=#f38f4e>" + parameters[1] + "</color> )<sprite index=" + spriteIndex + ">  <color=#98f460>+=</color>  " + parameters[2];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        EffectManager.Instance.CreateEffect(EffectTypes.powerLaser, agent.gridCoords, parameters, pointHit, true);
    }
    public override string ToolTip()
    {
        int spriteIndex = 8;
        if (parameters[0] == 0)
        {
            if (parameters[1] < 0)
            {
                spriteIndex = 3;
            }
            else if (parameters[1] > 0)
            {
                spriteIndex = 2;
            }
        }
        else if (parameters[1] == 0)
        {
            if (parameters[0] < 0)
            {
                spriteIndex = 1;
            }
            else if (parameters[0] > 0)
            {
                spriteIndex = 0;
            }
        }
        else
        {
            if (parameters[0] > 0)
            {
                if (parameters[1] > 0)
                {
                    spriteIndex = 5;
                }
                else if (parameters[1] < 0)
                {
                    spriteIndex = 6;
                }
            }
            else if (parameters[0] < 0)
            {
                if (parameters[1] > 0)
                {
                    spriteIndex = 4;
                }
                else if (parameters[1] < 0)
                {
                    spriteIndex = 7;
                }
            }
        }
        return "This unit <color=#98f460>adds</color> " + parameters[2] + " <color=#ebca54>PWR</color><sprite index=9> to the first unit in the direction ( <color=#4eb2f3>" + parameters[0] + "</color> , <color=#f38f4e>" + parameters[1] + "</color> )<sprite index=" + spriteIndex + "> .";
    }
}