using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XLessWhileBlock : WhileBlock
{
    public XLessWhileBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return agent.gridCoords.x < parameters[0];
    }

    public override string ShowSyntax()
    {
        return "While <color=#4eb2f3><b> X</b></color>  <  " + parameters[0];
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }
}