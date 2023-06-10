using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

public class AICharacterManager : CharacterManager
{
    private AICharacterWeaponAimer _aiCharacterWeaponAimer;
    private Tree _behaviorTree;
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _path;
    private bool _isIdle;

    public override void Initialize(Character character)
    {
        base.Initialize(character);
        _aiCharacterWeaponAimer = transform.Find("WeaponParent").GetComponent<AICharacterWeaponAimer>();
        _behaviorTree = GetComponent<Tree>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _path = new NavMeshPath();
        
    }

    private void Update()
    {
        DrawPath();
    }

    public override void ReceiveHit(Transform attacker, Vector2 origin)
    {
        _behaviorTree.SetData("attackedFromOrigin", origin);
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
        _navMeshAgent.ResetPath();
        _path = new NavMeshPath();
    }

    public override void OnSpeedChange()
    {
        _navMeshAgent.speed = _character.Stats.speed;
    }
    
    public void SetIdleStatus(bool status)
    {
        _isIdle = status;
    }

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public bool IsIdle => _isIdle;
    public AICharacterWeaponAimer AICharacterWeaponAimer => _aiCharacterWeaponAimer;
    public bool HasPath => _navMeshAgent.hasPath;
}
