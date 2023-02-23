using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum VCam { MainGridCam}

[System.Serializable]
public struct VirtualCameraStruct
{
    public VCam vCam;
    public CinemachineVirtualCamera obj;
}

public class CameraManager : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private List<VirtualCameraStruct> virtualCameraStructs;
    private Dictionary<VCam, CinemachineVirtualCamera> vCams;

    private Vector2 offset;

    public Vector2 Offset
    {
        get
        {
            return offset;
        }
        set
        {
            Vector2 delta = value - Offset;
            offset = value;
            MoveVCameraDelta(VCam.MainGridCam, delta);
        }
    }

    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        vCams = new Dictionary<VCam, CinemachineVirtualCamera>();
        foreach (VirtualCameraStruct v in virtualCameraStructs)
        {
            vCams.Add(v.vCam, v.obj);
        }

        offset = new Vector2(2.5f, 0);
    }

    private void OnEnable()
    {
        GridVisualizer.onNewCenter += MoveMainGridCamera;
    }

    private void OnDisable()
    {
        GridVisualizer.onNewCenter -= MoveMainGridCamera;
    }

    private void MoveMainGridCamera()
    {
        MoveVCamera(VCam.MainGridCam, Offset.x * Vector2.left);
    }

    private void MoveVCamera(VCam id, Vector2 pos)
    {
        CinemachineVirtualCamera current = vCams[id];
        current.transform.position = new Vector3(pos.x, pos.y, -20);
    }

    private void MoveVCameraDelta(VCam id, Vector2 delta)
    {
        CinemachineVirtualCamera current = vCams[id];
        current.transform.position = current.transform.position + (Vector3)delta;
    }
}
