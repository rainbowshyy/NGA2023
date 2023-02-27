using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEncounterPreviewElement : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    [SerializeField] private CanvasGroup plusSign;

    public void SetElement(Encounter encounter)
    {
        if (encounter.elements.Count > 4)
        {
            plusSign.alpha = 1;
        }
        else
        {
            plusSign.alpha = 0;
        }
        for (int i = 0; i < 4; i++)
        {
            if (i < encounter.elements.Count)
            {
                agentType type = encounter.elements[i].type;
                icons[i].sprite = AgentManager.Instance.AgentIconMap[type];
                icons[i].color = Color.white;
            }
            else
            {
                icons[i].color = Color.black;
            }
        }
    }

    public void SetEmpty()
    {
        plusSign.alpha = 0;
        for (int i = 0; i < 4; i++)
        {
            icons[i].sprite = null;
            icons[i].color = Color.black;
        }
    }
}
