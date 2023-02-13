using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentUI : MonoBehaviour
{
    public Canvas canvas;
    private RectTransform rectTf;

    private void Awake()
    {
        rectTf = GetComponent<RectTransform>();
    }

    public void SetPositionFromWorld(Vector3 pos)
    {
        rectTf.anchoredPosition = pos * canvas.scaleFactor * 64f + Vector3.down * 32f;
    }
}
