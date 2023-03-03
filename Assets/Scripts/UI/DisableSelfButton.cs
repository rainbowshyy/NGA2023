using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableSelfButton : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    public void Press(int id)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == id)
            {
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].interactable = true;
            }
        }
    }
}
