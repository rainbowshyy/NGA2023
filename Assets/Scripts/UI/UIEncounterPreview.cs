using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEncounterPreview : MonoBehaviour
{
    [SerializeField] private UIEncounterPreviewElement[] elements;

    private void OnEnable()
    {
        GameManager.onNewRoundStarted += ShowPreview;
    }

    private void OnDisable()
    {
        GameManager.onNewRoundStarted -= ShowPreview;
    }

    private void ShowPreview()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (i >= EncounterManager.Instance.currentPool.Count)
            {
                elements[i].SetEmpty();
            }
            else
            {
                elements[i].SetElement(EncounterManager.Instance.currentPool[i]);
            }
        }
    }
}
