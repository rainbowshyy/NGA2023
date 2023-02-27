using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum agentTeam { Player, Enemy }

public class CodeBlockAgent : GridElement
{
    public agentTeam team;
    public agentType type;

    public int health;
    public int energy;
    public List<int> currentStep;

    public override void Start()
    {
        base.Start();
        health = AgentManager.Instance.AgentHealthMap[type];
        energy = 0;
        currentStep = new List<int>() { 0 };

        UpdateStatsUI();
    }

    private void OnEnable()
    {
        CodeBlockManager.onDoCode += DoCode;
    }

    private void OnDisable()
    {
        CodeBlockManager.onDoCode -= DoCode;
    }

    private void DoCode(agentTeam codeTeam, int step)
    {
        if (codeTeam != team || !CodeBlockManager.Instance.TypeHasCode(type))
        {
            return;
        }

        bool isCondition = true;
        int depth = 0;
        CodeBlock current = CodeBlockManager.Instance.codeBlocks[type][currentStep[depth]];
        int safety = 0;
        bool alreadyLooped = false; //if it loops twice, there is no code to run.
        while (isCondition)
        {
            safety += 1;
            if (current.isCondition)                                    //Check if condition
            {
                if (currentStep.Count < depth + 2)                  //Make sure we are not trying to find index depth when it does not exist
                {
                    currentStep.Add(0);
                }
                if ((!current.RunCode(this) && currentStep[depth + 1] == 0) || current.scope == null || current.scope.Count <= 0)  //Check if there is no code to run in scope
                {
                    if (!current.RunCode(this) && currentStep[depth + 1] == 0)
                    {
                        current.VisualCode(this);
                    }
                    currentStep[depth] += 1;
                    if (depth == 0 && currentStep[depth] >= CodeBlockManager.Instance.codeBlocks[type].Count) //Loop if depth is 0
                    {
                        currentStep[depth] = 0;
                        if (alreadyLooped)
                        {
                            isCondition = false;
                            return;
                        }
                        alreadyLooped = true;
                    }
                    depth = 0;
                    current = CodeBlockManager.Instance.codeBlocks[type][currentStep[depth]];
                }
                else
                {
                    if (current.RunCode(this) && currentStep[depth + 1] == 0)
                    {
                        current.VisualCode(this);
                    }
                    depth += 1;                                         //If it is condition, add depth
                    if (currentStep.Count < depth + 1)                  //Make sure we are not trying to find index depth when it does not exist
                    {
                        currentStep.Add(0);
                    }
                    if (currentStep[depth] >= current.scope.Count)      //if depth index is greater than scope count, go back up in depth, and proceed to next scope/block
                    {
                        currentStep[depth] = 0;
                        if (!current.isLoop || !current.RunCode(this))
                        {
                            depth -= 1;
                            currentStep[depth] += 1;
                            if (depth == 0 && currentStep[depth] >= CodeBlockManager.Instance.codeBlocks[type].Count) //Loop if depth is 0
                            {
                                currentStep[depth] = 0;
                                if (alreadyLooped)
                                {
                                    isCondition = false;
                                    return;
                                }
                                alreadyLooped = true;
                            }
                            depth = 0;
                            current = CodeBlockManager.Instance.codeBlocks[type][currentStep[depth]];
                        }
                    }
                    else
                    {
                        current = current.scope[currentStep[depth]];        //Update currentblock to be the next in scope with new index, found by depth
                    }
                }
            }
            else
            {
                isCondition = false;
                current.RunCode(this);
                current.VisualCode(this);
                currentStep[depth] += 1;
                if (depth == 0 && currentStep[depth] >= CodeBlockManager.Instance.codeBlocks[type].Count) //Loop if depth is 0
                {
                    currentStep[depth] = 0;
                }
            }
            if (safety > 99)
            {
                isCondition = false;
                Debug.Log("Could not exit while loop");
                return;
            }
        }

        /*
        CodeBlockManager.Instance.codeBlocks[type][currentStep[0]].RunCode(this);
        currentStep[0] += 1;
        if (currentStep[0] >= CodeBlockManager.Instance.codeBlocks[type].Count)
        {
            currentStep[0] = 0;
        }
        */

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
        UpdateStatsUI();
        if (UI == null)
        {
            return;
        }
        EffectManager.Instance.CreateEffect(EffectTypes.energyUp, gridCoords, new int[2] {toAdd, 0}, Vector2Int.zero, true);
        UI.EnergyAnimation();
    }

    public void AddHealth(int toAdd)
    {
        health += toAdd;
        if (health <= 0)
        {
            OnRemove();
        }
        UpdateStatsUI();
        if (UI == null)
        {
            return;
        }
        if (toAdd > 0)
        {
            EffectManager.Instance.CreateEffect(EffectTypes.healthUp, gridCoords, new int[2] { toAdd, 0 }, Vector2Int.zero, true);
        }
        UI.HealthAnimation();
    }

    public void OnRemove()
    {
        GridManager.Instance.RemoveGridElement(this);
        if (team == agentTeam.Enemy)
        {
            SpawningManager.Instance.ChangeEnemyCount(-1);
        }
        else
        {
            SpawningManager.Instance.ChangePlayerCount(-1);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(UI.gameObject);
    }
}
