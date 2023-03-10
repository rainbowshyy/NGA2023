using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitBlock : CodeBlock
{
    public WaitBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override string ShowSyntax()
    {
        return "Wait ( )";
    }

    public override void VisualCode(CodeBlockAgent agent)
    {
        return;
    }

    public override string ToolTip()
    {
        return "This unit does nothing this tick.";
    }
}