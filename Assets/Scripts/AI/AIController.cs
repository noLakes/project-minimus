using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Character _character;
    private CharacterManager _characterManager;
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _path;
    private bool _isIdle;
    
    private void Start()
    {
        _characterManager = GetComponent<CharacterManager>();
        _character = _characterManager.Character;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _path = new NavMeshPath();
    }
    
    private void Update()
    {
        DrawPath();
    }

    public bool TryMove(Vector2 point)
    {
        // try move
        var hasPath = ValidPathTo(point);
        if(hasPath) FollowPath(_path);
        return hasPath;
    }

    private void FollowPath(NavMeshPath path)
    {
        _navMeshAgent.SetPath(path);
    }

    private bool SetDestination(Vector2 point)
    {
        return _navMeshAgent.SetDestination(point);
    }
    
    public bool ValidPathTo(Vector2 location)
    {
        return NavMesh.CalculatePath(transform.position, location, _navMeshAgent.areaMask, _path);
    }
    
    public bool ValidPathTo(Transform target) => ValidPathTo(target.position);

    private void DrawPath()
    {
        for (var i = 0; i < _path.corners.Length - 1; i++)
        {
            Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
        }
    }
    
    public void StopMoving()
    {
        // implement
    }

    public void SetIdleStatus(bool status)
    {
        _isIdle = status;
    }

    public void OnSleep()
    {
        // respond to BT sleeping
    }

    public Character Character => _character;
    public CharacterManager CharacterManager => _characterManager;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public bool IsIdle => _isIdle;
}
