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
    [SerializeField] private List<VirtualCameraStruct> virtualCameraStructs;
    private Dictionary<VCam, CinemachineVirtualCamera> vCams;

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

        GridVisualizer.onNewCenter += MoveMainGridCamera;
    }

    private void Start()
    {

    }

    private void MoveMainGridCamera(Vector2 pos)
    {
        MoveVCamera(VCam.MainGridCam, pos);
    }

    private void MoveVCamera(VCam id, Vector2 pos)
    {
        CinemachineVirtualCamera current = vCams[id];
        current.transform.position = new Vector3(pos.x, pos.y, -20);
    }
}
