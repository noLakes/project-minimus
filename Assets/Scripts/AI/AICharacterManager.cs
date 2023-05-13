using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    private AIWeaponAimManager _aiWeaponAimManager;
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _path;
    private bool _isIdle;
    
    private void Start()
    {
        _aiWeaponAimManager = transform.Find("WeaponParent").GetComponent<AIWeaponAimManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _path = new NavMeshPath();
    }
    
    private void Update()
    {
        DrawPath();
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.ResetPath();
            _path = new NavMeshPath();
        }
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
        var validity = NavMesh.CalculatePath(transform.position, location, _navMeshAgent.areaMask, _path);
        return validity;
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

    public bool TryAttack(Vector2 point)
    {
        // check line of sight? or do that in ai BT?
        _aiWeaponAimManager.AimTowards(point);
        Attack(point);
        return true;
    }

    public void SetIdleStatus(bool status)
    {
        _isIdle = status;
    }

    public void OnSleep()
    {
        // respond to BT sleeping
    }
    
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public bool IsIdle => _isIdle;
}
