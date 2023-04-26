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
    // used by cinemachine to apply offset and modify camera position
    private CinemachineTransposer _transposer;
    private Transform _playerTransform;
    private bool _followingTarget;
    
    // store values for camera view size
    private float _fovSizeX, _fovSizeY;
    // max offset allowed for camera (percentage of fov size)
    [SerializeField, Range(0f, 0.7f)] private float maxXOffsetPercent, maxYOffsetPercent;
    private float _maxCameraOffsetX, _maxCameraOffsetY;
    
    void Start()
    {
        _playerTransform = Game.Instance.PlayerCharacter.transform;
        CameraFollow(_playerTransform);
        
        // get screen size bounds for offset
        _fovSizeY = Camera.main.orthographicSize * 2;
        _maxCameraOffsetY = _fovSizeY * maxYOffsetPercent;
        _fovSizeX = _fovSizeY * Screen.width / Screen.height;
        _maxCameraOffsetX = _fovSizeX * maxXOffsetPercent;
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

    private void CameraFollow(Transform target)
    {
        cinemachineVirtualCamera.m_Follow = target;
        
        // setup _transposer - only available when component has a follow target
        _transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        _followingTarget = true;
    }

    private void ApplyCameraMouseOffset(Vector2 mouseDistance)
    {
        _transposer.m_FollowOffset = new Vector3(
            _maxCameraOffsetX * (-mouseDistance.x / _fovSizeX),
            _maxCameraOffsetY * (-mouseDistance.y / _fovSizeY),
            -10f
        );
    }

    private void ResetCameraPosition()
    {
        _transposer.m_FollowOffset.x = 0f;
        _transposer.m_FollowOffset.y = 0f;
        cinemachineVirtualCamera.m_Follow = null;
        _transposer = null;
        _followingTarget = false;
    }
}
