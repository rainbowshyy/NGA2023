using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UICameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform UIwindow;
    [SerializeField] private Canvas canvas;

    [SerializeField] private bool windowVisible;
    [SerializeField] private RectTransform windowContainer;
    [SerializeField] private float windowScrollSpeed;

    [SerializeField] private float cameraMoveFactor;

    private Vector2 lastPos;
    private Vector2 toMove;
    private bool doMove;

    private void Awake()
    {
        toMove = Vector2.zero;
    }

    private void Start()
    {
        InputManager.onMousePos += DoMousePos;
        InputManager.onRightMouse += DoRightMouse;
    }

    private void Update()
    {
        //vCam.transform.position += (Vector3)toMove;
        background.anchoredPosition += toMove;
        toMove = Vector2.zero;

        if (windowVisible && windowContainer.anchoredPosition.x < 0)
        {
            windowContainer.anchoredPosition += windowScrollSpeed * Time.deltaTime * Vector2.right;
            if (windowContainer.anchoredPosition.x > 0)
            {
                windowContainer.anchoredPosition = Vector2.zero;
            }
        }
        else if (!windowVisible && windowContainer.anchoredPosition.x > -1000)
        {
            windowContainer.anchoredPosition += windowScrollSpeed * Time.deltaTime * Vector2.left;
            if (windowContainer.anchoredPosition.x < -1000)
            {
                windowContainer.anchoredPosition = Vector2.left * 1000;
            }
        }
    }

    private void DoMousePos(Vector2 pos)
    {
        /*
        Vector2 newPos = pos * ((vCam.m_Lens.OrthographicSize * 1.6f) / cam.pixelHeight);
        if (doMove)
        {
            toMove += lastPos - newPos;
        }
        lastPos = newPos;
        */
        Vector2 newPos = pos / canvas.scaleFactor;
        if (doMove && windowVisible)
            toMove += (newPos - lastPos);
        lastPos = newPos;

        if (newPos.x > 880 && windowVisible)
            windowVisible = false;
        else if (newPos.x < 80 && !windowVisible)
        {
            windowVisible = true;
        }
    }

    private void DoRightMouse(bool set)
    {
        doMove = set;
    }

    public void ToggleWindow(bool toggle)
    {

    }
}
