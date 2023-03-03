using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutRebuilderElement : MonoBehaviour
{
    public void DoRebuild()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        LayoutRebuilderElement comp = transform.parent.gameObject.GetComponent<LayoutRebuilderElement>();
        if (comp != null )
        {
            comp.DoRebuild();
        }
    }
}
