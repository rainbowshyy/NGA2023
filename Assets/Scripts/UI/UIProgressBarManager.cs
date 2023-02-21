using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProgressBarManager : MonoBehaviour
{
    [SerializeField] private Transform progressBarParent;

    public static UIProgressBarManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetAllInactive();
    }

    private void Start()
    {
        CodeBlockManager.onDoCode += SetStep;
    }

    public void SetStep(agentTeam team, int step)
    {
        progressBarParent.GetChild(step).gameObject.GetComponent<UIProgressBarElement>().SetActive(true, 35 - step);
    }

    public void SetAllInactive()
    {
        foreach (Transform t in progressBarParent)
        {
            t.gameObject.GetComponent<UIProgressBarElement>().SetActive(false, 0);
        }
    }
}
