using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum agentTeam { Player, Enemy}
public enum agentType { Blue}

public class CodeBlockAgent : GridElement
{
    public agentTeam team;
    public agentType type;

    public void AssignTeam()
    {

    }

    public override void Start()
    {
        base.Start();
        CodeBlockManager.onDoCode += DoCode;
    }

    private void DoCode(agentTeam codeTeam, int step)
    {
        if (codeTeam != team)
        {
            return;
        }

        CodeBlockManager.Instance.codeBlocks[type][CodeBlockManager.Instance.GetStepForType(type, step)].RunCode(this);
    }
}
