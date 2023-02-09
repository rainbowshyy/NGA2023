using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool canBeDropped;

    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform anchorToMove;
    public DropZone dropZone;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool dragged = false;

    private void Start()
    {
        DragDropManager.onBeginDrag += DoBeginDrag;
        DragDropManager.onStopDrag += DoStopDrag;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(anchorToMove, eventData.position, eventData.pressEventCamera, out var globalMousePos))
        {
            anchorToMove.position = globalMousePos;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }

        Transform oldParent = anchorToMove.parent;
        anchorToMove.SetParent(background);
        anchorToMove.SetSiblingIndex(background.childCount - 1);
        dragged = true;
        dropZone.dragged = true;
        canvasGroup.blocksRaycasts = false;
        if (canBeDropped)
            dropZone.dropTransform = null;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)oldParent);
        DragDropManager.onBeginDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }

        dragged = false;
        dropZone.dragged = false;
        canvasGroup.blocksRaycasts = true;
        DragDropManager.onStopDrag?.Invoke();
    }

    private void DoBeginDrag()
    {
        if (!dragged)
        {
            gameObject.SetActive(false);
        }
    }

    private void DoStopDrag()
    {
        gameObject.SetActive(true);
    }

    public void SetAnchorParent(RectTransform tf, int index)
    {
        if (canBeDropped)
        {
            anchorToMove.SetParent(tf);
            anchorToMove.SetSiblingIndex(index);
            LayoutRebuilder.ForceRebuildLayoutImmediate(tf); //update layoutgroup?
        }
    }
}
