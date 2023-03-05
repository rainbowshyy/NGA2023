using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject text;
    [SerializeField] private Button stopButton;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        InputManager.onToggleInputs += ToggleActive;
        ToggleActive(true);
    }

    private void OnDisable()
    {
        InputManager.onToggleInputs -= ToggleActive;
    }

    private void ToggleActive(bool active)
    {
        button.interactable = active;
        button.image.enabled = active;
        text.SetActive(active);
        stopButton.gameObject.SetActive(!active);
        stopButton.interactable = !active;
    }
}
