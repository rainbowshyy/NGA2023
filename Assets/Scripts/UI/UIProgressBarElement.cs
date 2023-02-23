using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIProgressBarElement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    public void SetActive(bool active, int left, bool first)
    {
        if (active)
        {
            image.sprite = activeSprite;
            text.text = left + " left";
        }
        else
        {
            image.sprite = inactiveSprite;
        }
        if (!active || !first)
        {
            text.text = "";
        }
    }
}
