using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyRayBlock : ConditionBlock
{
    public EnemyRayBlock(int[] param) : base(param) { conditionType = ConditionOptionType.Direction; }

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
                    return agentHit.team != agent.team;
                }
                return false;
            }
        }
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
        return "If Enemy at Raycast( <color=#4eb2f3>" + parameters[0] + "</color> , <color=#f38f4e>" + parameters[1] + "</color> )<sprite index=" + spriteIndex + ">";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
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
        return "Runs this block's nested codeblocks if there is an enemy unit in the direction ( <color=#4eb2f3>" + parameters[0] + "</color> , <color=#f38f4e>" + parameters[1] + "</color> )<sprite index=" + spriteIndex + "> . A unit is an enemy if it is not on the same team as this unit.";
    }
}
