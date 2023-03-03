using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeBox : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    public void Toggle(bool toggle)
    {
        GetComponent<Animator>().SetBool("Visible", toggle);
        buttons[0].gameObject.SetActive(!toggle);
        buttons[1].gameObject.SetActive(toggle);
    }
}
