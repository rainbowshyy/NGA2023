using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SellDrop : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject symbols;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CodeBox shopManager;
    private bool openedShop;

    private void OnEnable()
    {
        DragDropManager.onBeginDrag += DoBeginDrag;
        DragDropManager.onStopDrag += DoStopDrag;
        ShopManager.onCodePickup += SetInfo;
    }

    private void OnDisable()
    {
        DragDropManager.onBeginDrag -= DoBeginDrag;
        DragDropManager.onStopDrag -= DoStopDrag;
        ShopManager.onCodePickup -= SetInfo;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<DragDrop>() != null)
        {
            UICodeBlock block = eventData.pointerDrag.transform.parent.GetComponent<UICodeBlock>();
            if (block != null)
            {
                GameManager.Instance.AddGold(Mathf.CeilToInt(block.Code.price / 2f));
                eventData.pointerDrag.GetComponent<DragDrop>().sold = true;
            }
        }
    }

    private void DoBeginDrag()
    {
        image.enabled = true;
        symbols.SetActive(true);
        if (!shopManager.visible)
        {
            shopManager.Toggle(true);
            openedShop = true;
        }
    }

    private void DoStopDrag()
    {
        image.enabled = false;
        symbols.SetActive(false);
        if (openedShop && shopManager.visible)
        {
            shopManager.Toggle(false);
            openedShop = false;
        }
    }

    private void SetInfo(CodeBlock code)
    {
        text.text = "Sell for " + Mathf.CeilToInt(code.price / 2f) + " <sprite index=10>";
    }
}
