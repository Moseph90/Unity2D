using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCamera : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (GameManager.Instance.playerInstance == null) return;

        vcam.LookAt = GameManager.Instance.playerInstance.transform;
        vcam.Follow = GameManager.Instance.playerInstance.transform;
    }
}
