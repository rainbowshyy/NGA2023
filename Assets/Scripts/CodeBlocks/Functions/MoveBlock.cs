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
        return agent.TryMove(parameters[0], parameters[1]);
    }

    public override string ShowSyntax()
    {
        return "Move(<color=#FF3939><b>" + parameters[0].ToString() + "</b></color>, <color=#28FF4F><b>" + parameters[1].ToString() + "</b></color>);";
    }
}