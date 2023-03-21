using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;

    void Start()
    {
        CameraFollow(Game.Instance.PlayerCharacter.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraFollow(Transform target)
    {
        cinemachineVirtualCamera.m_Follow = target;
    }
}
