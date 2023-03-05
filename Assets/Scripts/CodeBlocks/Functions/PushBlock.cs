using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : CodeBlock
{
    public PushBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        List<CodeBlockAgent> agentsInLine = new List<CodeBlockAgent>();
        bool searching = true;
        bool valid = true;
        Vector2Int currentPos = agent.gridCoords;
        while (searching)
        {
            currentPos += new Vector2Int(parameters[0], parameters[1]);
            CodeBlockAgent current = GridManager.Instance.GetAgentAtCoords(currentPos.x, currentPos.y);
            if (current != null)
            {
                agentsInLine.Add(current);
            }
            else
            {
                searching = false;
                if (currentPos.x > 6 || currentPos.x < 0 || currentPos.y < 0 || currentPos.y > 6)
                {
                    valid = false;
                    return valid;
                }
            }
        }

        for (int i = 0; i < agentsInLine.Count; i++)
        {
            CodeBlockAgent current = agentsInLine[agentsInLine.Count - 1 - i];
            current.TryMove(parameters[0], parameters[1]);
        }

        agent.TryMove(parameters[0], parameters[1]);

        return valid;
    }

    public override string ShowSyntax()
    {
        //refactor this i think :)
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
            return "Push( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> )";
        }

        int greatestNumber;
        if (Mathf.Abs(parameters[0]) > Mathf.Abs(parameters[1]))
        {
            greatestNumber = Mathf.Abs(parameters[0]);
        }
        else
        {
            greatestNumber = Mathf.Abs(parameters[1]);
        }

        string text = "Push( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> )<sprite index=" + spriteIndex + ">";
        return text;
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
        return "This unit moves <color=#4eb2f3>" + parameters[0] + "</color> tiles along the <color=#4eb2f3>X axis</color> and <color=#f38f4e>" + parameters[1] + "</color> tiles along the <color=#f38f4e>Y axis</color> ( <sprite index=" + spriteIndex + "> ) and pushes any number of colliding units if able.";
    }
}