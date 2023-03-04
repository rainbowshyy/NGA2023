using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIUpgradeElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI text;
    private string toolTip;
    private bool hover;
    private bool showed;
    private float hoverTimeCurrent;
    [SerializeField] private float hoverTimeNeeded;

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void SetToolTip(string text)
    {
        toolTip = text;
    }

    private void Update()
    {
        if (hover)
        {
            if (hoverTimeCurrent < hoverTimeNeeded)
            {
                hoverTimeCurrent += Time.deltaTime;
            }
            else if (!showed)
            {
                TooltipBox.onShowTooltip?.Invoke(transform.position, toolTip);
                showed = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
        showed = false;
        hoverTimeCurrent = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        TooltipBox.onHideTooltip?.Invoke();
        hoverTimeCurrent = 0;
    }
}
