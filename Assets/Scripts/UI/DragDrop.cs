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
    [SerializeField] private LayoutElement layoutElement;

    private Transform oldParent;
    private int lastSiblingIndex;

    private bool dragged = false;

    private bool inputsEnabled;

    public bool sold;

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

        oldParent = anchorToMove.parent;
        if (oldParent == background)
            lastSiblingIndex = anchorToMove.GetSiblingIndex();
        anchorToMove.SetParent(background.parent.parent);
        anchorToMove.SetSiblingIndex(background.parent.parent.childCount - 1);
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

        LayoutRebuilderElement comp = oldParent.gameObject.GetComponent<LayoutRebuilderElement>();
        if (comp != null)
        {
            comp.DoRebuild();
        }

        DragDropManager.onBeginDrag?.Invoke();
        ShopManager.onCodePickup?.Invoke(transform.parent.GetComponent<UICodeBlock>().Code);
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

        if (anchorToMove.parent == background.parent.parent)
        {
            SetAnchorParent(background, lastSiblingIndex);
        }

        DragDropManager.onStopDrag?.Invoke();
        if (sold)
        {
            Destroy(anchorToMove.gameObject);
        }
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
            if (index < anchorToMove.parent.childCount)
            {
                anchorToMove.SetSiblingIndex(index);
            }
            else
            {
                anchorToMove.SetAsLastSibling();
            }
            //EditorGUIUtility.PingObject(tf.gameObject);
            //LayoutRebuilder.ForceRebuildLayoutImmediate(tf); //update layoutgroup?
            LayoutRebuilderElement comp = tf.gameObject.GetComponent<LayoutRebuilderElement>();
            if (comp != null)
            {
                comp.DoRebuild();
            }
        }
    }

    public void SetBackground(RectTransform rt)
    {
        background = rt;
    }
}