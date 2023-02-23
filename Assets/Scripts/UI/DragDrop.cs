using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool canBeDropped;

    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform anchorToMove;
    public List<DropZone> dropZone;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool dragged = false;

    private bool inputsEnabled;

    private void Start()
    {
        ToggleInputsEnabled(true);
    }

    private void OnEnable()
    {
        DragDropManager.onBeginDrag += DoBeginDrag;
        DragDropManager.onStopDrag += DoStopDrag;
        InputManager.onToggleInputs += ToggleInputsEnabled;
    }

    private void OnDisable()
    {
        DragDropManager.onBeginDrag -= DoBeginDrag;
        DragDropManager.onStopDrag -= DoStopDrag;
        InputManager.onToggleInputs -= ToggleInputsEnabled;
    }

    private void ToggleInputsEnabled(bool enabled)
    {
        inputsEnabled = enabled;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right || (!inputsEnabled && canBeDropped))
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
        if (eventData.button == PointerEventData.InputButton.Right || (!inputsEnabled && canBeDropped))
        {
            return;
        }

        Transform oldParent = anchorToMove.parent;
        anchorToMove.SetParent(background);
        anchorToMove.SetSiblingIndex(background.childCount - 1);
        dragged = true;
        canvasGroup.blocksRaycasts = false;
        if (canBeDropped && dropZone[0] != null)
            dropZone[0].dropTransform = null;
        foreach (DropZone d in dropZone)
        {
            if (d != null)
            {
                d.dragged = true;
            }
        }
        //LayoutRebuilder.MarkLayoutForRebuild((RectTransform)oldParent);
        //EditorGUIUtility.PingObject(oldParent.gameObject);
        //LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)oldParent);
        LayoutRebuilderManager.Rebuild?.Invoke();
        DragDropManager.onBeginDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right || (!inputsEnabled && canBeDropped))
        {
            return;
        }

        dragged = false;
        foreach (DropZone d in dropZone)
        {
            if (d != null)
                d.dragged = false;
        }
        canvasGroup.blocksRaycasts = true;
        DragDropManager.onStopDrag?.Invoke();
    }

    private void DoBeginDrag()
    {
        if (!dragged)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void DoStopDrag()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void SetAnchorParent(RectTransform tf, int index)
    {
        if (canBeDropped)
        {
            anchorToMove.SetParent(tf);
            anchorToMove.SetSiblingIndex(index);
            //EditorGUIUtility.PingObject(tf.gameObject);
            //LayoutRebuilder.ForceRebuildLayoutImmediate(tf); //update layoutgroup?
            LayoutRebuilderManager.Rebuild?.Invoke();
        }
    }

    public void SetBackground(RectTransform rt)
    {
        background = rt;
    }
}