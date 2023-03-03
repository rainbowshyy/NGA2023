using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentWindowManager : MonoBehaviour
{
    [SerializeField] private Transform windowParent;

    [SerializeField] private GameObject windowPref;

    private Dictionary<agentType, GameObject> windowMap;

    public static AgentWindowManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        windowMap = new Dictionary<agentType, GameObject>();
    }

    public GameObject AddWindow(agentType agentType, bool enemy)
    {
        if (windowMap.ContainsKey(agentType))
        {
            return windowMap[agentType];
        }

        GameObject go = Instantiate(windowPref, windowParent);

        if (!enemy)
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
        }

        windowMap.Add(agentType, go);

        return go;
    }

    public void OpenWindow(agentType agentType, bool open)
    {
        if (!windowMap.ContainsKey(agentType))
        {
            return;
        }

        CloseWindows();

        windowMap[agentType].GetComponent<Animator>().SetBool("Visible", open);
        if (open)
            Debug.Log(agentType);
    }

    public void CloseWindows()
    {
        foreach (GameObject go in windowMap.Values)
        {
            go.GetComponent<Animator>().SetBool("Visible", false);
        }
    }
}
