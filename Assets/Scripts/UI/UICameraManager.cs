using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UICameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform background;
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
        Bounds boxBounds = new Bounds(Vector3.zero, new Vector3(360f, 160f, 0));
        if (!boxBounds.Contains(background.anchoredPosition))
        {
            background.anchoredPosition = boxBounds.ClosestPoint(background.anchoredPosition);
        }
        toMove = Vector2.zero;

        Vector2 step;
        if (windowVisible && CameraManager.Instance.Offset.x > -5f)
        {
            step = windowScrollSpeed * Time.deltaTime * Vector2.right;
            //windowContainer.anchoredPosition += step;
            CameraManager.Instance.Offset -= step / 64f;
            if (CameraManager.Instance.Offset.x < -5f)
            {
                //windowContainer.anchoredPosition = Vector2.zero;
                CameraManager.Instance.Offset = new Vector2(-5f, 0f);
            }
        }
        else if (!windowVisible && CameraManager.Instance.Offset.x < 2.5f)
        {
            step = windowScrollSpeed * Time.deltaTime * Vector2.left;
            //windowContainer.anchoredPosition += step;
            CameraManager.Instance.Offset -= step / 64f;
            if (CameraManager.Instance.Offset.x > 2.5f)
            {
                //windowContainer.anchoredPosition = Vector2.left * 360;
                CameraManager.Instance.Offset = new Vector2(2.5f, 0f);
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
        Vector2 newPos = new Vector2(pos.x / Screen.width, pos.y / Screen.height);
        if (doMove && windowVisible)
            toMove += (newPos * new Vector2(Screen.width, Screen.height) - lastPos);
        lastPos = newPos * new Vector2(Screen.width, Screen.height);

        if (newPos.x > 0.95f && windowVisible)
            windowVisible = false;
        else if (newPos.x < 0.05f && !windowVisible)
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
