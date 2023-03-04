using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAgentElement : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetAnimator(agentType type)
    {
        animator.runtimeAnimatorController = AgentManager.Instance.AgentAnimatorMap[type];
    }
}
