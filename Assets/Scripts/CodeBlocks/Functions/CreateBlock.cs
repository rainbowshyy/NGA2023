using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : CodeBlock
{
    public CreateBlock(int[] param) : base(param) { }

    public override bool ReadyCode(CodeBlockAgent agent)
    {
        return true;
    }

    public override bool RunCode(CodeBlockAgent agent)
    {
        if (agent.energy > parameters[2] && !GridManager.Instance.ElementAtTile(agent.gridCoords.x + parameters[0], agent.gridCoords.y + parameters[1]))
        {
            if (agent.team == agentTeam.Player)
            {
                SpawningManager.Instance.SpawnCodeAgent((agentType)parameters[3],new Vector2Int(agent.gridCoords.x + parameters[0], agent.gridCoords.y + parameters[1]));
            }
            else
            {
                SpawningManager.Instance.SpawnEnemy((agentType)parameters[3], new Vector2Int(agent.gridCoords.x + parameters[0], agent.gridCoords.y + parameters[1]));
            }
            agent.energy = 0;
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

        string name = AgentManager.Instance.AgentNameMap[(agentType)parameters[3]];
        string hexColor = ColorUtility.ToHtmlStringRGB(AgentManager.Instance.AgentColorMap[(agentType)parameters[3]]);

        string text = "If <color=#ebca54>PWR</color><sprite index=9>  >  " + parameters[2] + "  and  Space( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> )<sprite index=" + spriteIndex + ">" +
            "\n      Create new <color=#" + hexColor + ">" + name + "</color>( <color=#4eb2f3><b>" + parameters[0].ToString() + "</b></color> , <color=#f38f4e><b>" + parameters[1].ToString() + "</b></color> )<sprite index=" + spriteIndex + ">" +
            "\n      <color=#ebca54>PWR</color><sprite index=9>  =  0";

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
        string name = AgentManager.Instance.AgentNameMap[(agentType)parameters[3]];
        string hexColor = ColorUtility.ToHtmlStringRGB(AgentManager.Instance.AgentColorMap[(agentType)parameters[3]]);
        return "If this units <color=#ebca54>PWR</color><sprite index=9> is greater than " + parameters[2] + " and there is nothing <color=#4eb2f3>" + parameters[0] + "</color> tiles along the <color=#4eb2f3>X axis</color> and <color=#f38f4e>" + parameters[1] + "</color> tiles along the <color=#f38f4e>Y axis</color> ( <sprite index=" + spriteIndex + "> ), this unit <color=#98f460>creates a new</color> <color=#" + hexColor + ">" + name + "</color> . Then set this units <color=#ebca54>PWR</color><sprite index=9> to 0.";
    }
}