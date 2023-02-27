using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlock : ConditionBlock
{
    public SpaceBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return !GridManager.Instance.ElementAtTile(agent.gridCoords.x + parameters[0], agent.gridCoords.y + parameters[1]);
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

        if (spriteIndex == 8)
        {
            return "If Space ( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> )";
        }

        return "If Space ( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> ) <sprite index=" + spriteIndex + ">";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }
}