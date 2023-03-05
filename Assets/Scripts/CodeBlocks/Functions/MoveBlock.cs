using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : CodeBlock
{
    public MoveBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        int greatestNumber;
        if (Mathf.Abs(parameters[0]) > Mathf.Abs(parameters[1]))
        {
            greatestNumber = Mathf.Abs(parameters[0]);
        }
        else
        {
            greatestNumber = Mathf.Abs(parameters[1]);
        }

        for (int i = 0; i < greatestNumber; i++)
        {
            agent.TryMove(Mathf.RoundToInt(parameters[0] / greatestNumber), Mathf.RoundToInt(parameters[1] / greatestNumber));
        }

        return true;
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
            return "Move( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> )";
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

        string text = "Move( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> )";

        for (int i = 0; i < greatestNumber; i++)
        {
            text += "<sprite index=" + spriteIndex + ">";
        }
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
        return "This unit moves <color=#4eb2f3>" + parameters[0] + "</color> tiles along the <color=#4eb2f3>X axis</color> and <color=#f38f4e>" + parameters[1] + "</color> tiles along the <color=#f38f4e>Y axis</color> ( <sprite index=" + spriteIndex + "> ) if able.";
    }
}