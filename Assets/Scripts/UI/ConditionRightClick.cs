using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConditionRightClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UICodeBlock block;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ConditionBlock code = (ConditionBlock)block.Code;
            ConditionOptionWindow.onConditionOptionOpen?.Invoke(code.conditionType, block);
        }
    }
}
