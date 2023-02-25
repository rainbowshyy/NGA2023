using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpgradeElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(string newText)
    {
        text.text = newText;
    }
}
