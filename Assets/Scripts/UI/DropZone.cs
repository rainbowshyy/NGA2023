using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public bool codeParent;

    public RectTransform dropTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private int siblingIndex;

    public bool dragged;

    private void OnEnable()
    {
        DragDropManager.onBeginDrag += DoBeginDrag;
        DragDropManager.onStopDrag += DoStopDrag;
    }

    private void OnDisable()
    {
        DragDropManager.onBeginDrag-= DoBeginDrag;
        DragDropManager.onStopDrag-= DoStopDrag;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<DragDrop>().canBeDropped)
        {
            eventData.pointerDrag.GetComponent<DragDrop>().SetAnchorParent(dropTransform, siblingIndex);
            eventData.pointerDrag.GetComponent<DragDrop>().dropZone[0].dropTransform = dropTransform;
        }
    }

    private void DoBeginDrag()
    {
        if (!dragged && dropTransform != null)
        {
            canvasGroup.blocksRaycasts = true;
            if (!codeParent)
            {
                siblingIndex = transform.parent.GetSiblingIndex() + 1;
            }
        }
    }

    private void DoStopDrag()
    {
        canvasGroup.blocksRaycasts = false;
    }
}
