using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum agentTeam { Player, Enemy}
public enum agentType { Blue, Blob}

public class CodeBlockAgent : GridElement
{
    public agentTeam team;
    public agentType type;

    public int health;
    public int energy;

    public override void Start()
    {
        base.Start();
        health = 1;
        energy = 1;
        CodeBlockManager.onDoCode += DoCode;

        UpdateStatsUI();
    }

    private void DoCode(agentTeam codeTeam, int step)
    {
        if (codeTeam != team || !CodeBlockManager.Instance.TypeHasCode(type))
        {
            return;
        }
        CodeBlockManager.Instance.codeBlocks[type][CodeBlockManager.Instance.GetStepForType(type, step)].RunCode(this);
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        if (UI == null)
        {
            return;
        }
        UI.SetEnergyText(energy);
        UI.SetHPText(health);
    }

    public void AddEnergy(int toAdd)
    {
        energy += toAdd;
        if (energy < 0)
        {
            energy = 0;
        }
    }
}
