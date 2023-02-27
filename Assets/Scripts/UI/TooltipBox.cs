using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class TooltipBox : MonoBehaviour
{
    [SerializeField] private RectTransform rectTf;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private Animator animator;

    public static Action<Vector3, string> onShowTooltip;
    public static Action onHideTooltip;

    private void OnEnable()
    {
        onShowTooltip += ShowTooltip;
        onHideTooltip += HideTooltip;
    }

    private void OnDisable()
    {
        onHideTooltip -= HideTooltip;
        onShowTooltip -= ShowTooltip;
    }

    private void ShowTooltip(Vector3 pos, string text)
    {
        animator.SetBool("visible", true);
        textBox.text = text;
        transform.position = pos + Vector3.right * 0.5f;
    }

    private void HideTooltip()
    {
        animator.SetBool("visible", false);
    }
}
