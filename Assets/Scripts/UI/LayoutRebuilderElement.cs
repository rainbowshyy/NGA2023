using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutRebuilderElement : MonoBehaviour
{
    private void OnEnable()
    {
        LayoutRebuilderManager.Rebuild += MarkForRebuild;
    }

    private void OnDisable()
    {
        LayoutRebuilderManager.Rebuild -= MarkForRebuild;
    }

    private void MarkForRebuild()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
