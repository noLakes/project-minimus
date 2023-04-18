using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Diagnostics;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer transposer;
    private Transform _playerTransform;
    private bool _followingTarget;

    private float _fovSizeX;
    private float _maxCameraOffsetX;
    private float _fovSizeY;
    private float _maxCameraOffsetY;

    void Start()
    {
        _playerTransform = Game.Instance.PlayerCharacter.transform;
        CameraFollow(_playerTransform);
        
        // get screen size bounds for offset
        _fovSizeY = Camera.main.orthographicSize * 2;
        _maxCameraOffsetY = _fovSizeY * 0.7f;
        _fovSizeX = _fovSizeY * Screen.width / Screen.height;
        _maxCameraOffsetX = _fovSizeX * 0.7f;

        Debug.Log("fovSizeX: " + _fovSizeX);
        Debug.Log("maxCamOffsetX: " + _maxCameraOffsetX);
        Debug.Log("fovSizeY: " + _fovSizeY);
        Debug.Log("maxCamOffsetY: " + _maxCameraOffsetY);
    }
    
    void Update()
    {
        if (_followingTarget)
        {
            Vector2 mousePos = Utility.GetMouseWorldPosition2D();

            Vector2 mouseDistance = new Vector2(
                _playerTransform.position.x - mousePos.x,
                _playerTransform.position.y - mousePos.y
            );

            ApplyCameraMouseOffset(mouseDistance);
        }
    }

    public void CameraFollow(Transform target)
    {
        cinemachineVirtualCamera.m_Follow = target;
        
        // setup transposer - only available when component has a follow target
        transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        _followingTarget = true;
    }

    private void ApplyCameraMouseOffset(Vector2 mouseDistance)
    {
        Debug.Log(mouseDistance);
        
        transposer.m_FollowOffset = new Vector3(
            _maxCameraOffsetX * (-mouseDistance.x / _fovSizeX),
            _maxCameraOffsetY * (-mouseDistance.y / _fovSizeY),
            -10f
        );
    }

    private void ResetCameraPosition()
    {
        transposer.m_FollowOffset.x = 0f;
        transposer.m_FollowOffset.y = 0f;
        cinemachineVirtualCamera.m_Follow = null;
        transposer = null;
        _followingTarget = false;
    }
}
