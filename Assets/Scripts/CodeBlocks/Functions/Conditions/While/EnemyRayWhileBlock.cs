using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyRayWhileBlock : WhileBlock
{
    public EnemyRayWhileBlock(int[] param) : base(param) { }

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
            currentPos += new Vector2Int(parameters[0], parameters[1]);
            hit = GridManager.Instance.GetAgentAtCoords(currentPos.x, currentPos.y);
            if (hit != null)
            {
                return hit.team != agent.team;
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
        return "While Enemy in Raycast ( <color=#4eb2f3>" + parameters[0] + "</color> , <color=#f38f4e>" + parameters[1] + "</color> ) <sprite index=" + spriteIndex + ">";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }
}
