using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator effectAnimator;
    private int lastNumber = 5;

    private void OnEnable()
    {
        GameManager.onNewGold += SetText;
    }

    private void OnDisable()
    {
        GameManager.onNewGold -= SetText;
    }

    private void SetText(int number)
    {
        text.text = number.ToString();
        if (number - lastNumber > 0)
        {
            animator.ResetTrigger("AddGold");
            animator.SetTrigger("AddGold");
            effectAnimator.ResetTrigger("AddGold");
            effectAnimator.SetTrigger("AddGold");
        }
        else
        {
            animator.ResetTrigger("AddGold");
            animator.SetTrigger("AddGold");
        }
    }
}
