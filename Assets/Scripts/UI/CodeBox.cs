using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeBox : MonoBehaviour
{
    [SerializeField] private bool shop;
    [SerializeField] private Button[] buttons;

    public bool visible;

    private void OnEnable()
    {
        InputManager.onToggleInputs += InputToggle;
    }

    private void OnDisable()
    {
        InputManager.onToggleInputs -= InputToggle;
    }

    public void Toggle(bool toggle)
    {
        GetComponent<Animator>().SetBool("Visible", toggle);
        buttons[0].gameObject.SetActive(!toggle);
        buttons[1].gameObject.SetActive(toggle);
        visible = toggle;
    }

    private void InputToggle(bool toggle)
    {
        if (!toggle || (toggle && shop))
        {
            GetComponent<Animator>().SetBool("Visible", toggle);
            buttons[0].gameObject.SetActive(!toggle);
            buttons[1].gameObject.SetActive(toggle);
        }
        buttons[0].interactable = toggle;
    }
}
