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

    public void SetActive(bool active, int left)
    {
        if (active)
        {
            image.sprite = activeSprite;
            text.text = left + " left";
        }
        else
        {
            image.sprite = inactiveSprite;
            text.text = "";
        }   
    }
}
