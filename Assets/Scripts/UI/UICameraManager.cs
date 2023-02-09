using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UICameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;

    private Vector2 toMove;

    private void Awake()
    {
        toMove = Vector2.zero;
    }

    private void Start()
    {
        InputManager.onMouseDelta += DoMouseDelta;
    }

    private void Update()
    {
        vCam.transform.position += (Vector3)toMove;
        toMove = Vector2.zero;
    }

    private void DoMouseDelta(Vector2 delta)
    {
        Debug.Log(delta);
        toMove += delta;
    }
}
