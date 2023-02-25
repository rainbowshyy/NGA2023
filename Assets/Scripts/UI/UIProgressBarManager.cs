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

    private void OnEnable()
    {
        CodeBlockManager.onDoCode += SetStep;
        CodeBlockManager.onStartProgram += SetAllInactive;
        GameManager.onNewRound += SetAllInactive;
    }

    private void OnDisable()
    {
        CodeBlockManager.onDoCode -= SetStep;
        CodeBlockManager.onStartProgram -= SetAllInactive;
        GameManager.onNewRound -= SetAllInactive;
    }

    public void SetStep(agentTeam team, int step)
    {
        progressBarParent.GetChild(step).gameObject.GetComponent<UIProgressBarElement>().SetActive(true, 35 - step, true);
        if (step != 0)
        {
            progressBarParent.GetChild(step - 1).gameObject.GetComponent<UIProgressBarElement>().SetActive(true, 35 - step, false);
        }
    }

    public void SetAllInactive()
    {
        foreach (Transform t in progressBarParent)
        {
            t.gameObject.GetComponent<UIProgressBarElement>().SetActive(false, 0, false);
        }
    }
}
