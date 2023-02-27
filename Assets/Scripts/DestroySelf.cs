using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null && GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        if (animator != null)
        {
            delay += animator.GetCurrentAnimatorStateInfo(0).length;
        }
        Destroy(gameObject, delay);
    }
}
