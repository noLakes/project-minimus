using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

public class AICharacterManager : CharacterManager
{
    private AIWeaponAimManager _aiWeaponAimManager;
    private Tree _behaviorTree;
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _path;
    private bool _isIdle;

    private void Start()
    {
        _aiWeaponAimManager = transform.Find("WeaponParent").GetComponent<AIWeaponAimManager>();
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
        object currentTarget = _behaviorTree.GetData("currentTarget");
        if (currentTarget != null) return; // already has target
        InvestigatePointDirection(origin);
    }

    private void InvestigatePointDirection(Vector2 point)
    {
        Vector2 investigateDir = (point - (Vector2)transform.position).normalized;

        Vector2 investigatePoint = (Vector2)transform.position + (investigateDir * _character.Stats.fovRadius);
        
        if (!ValidPathTo(investigatePoint))
        {
            investigatePoint = Utility.GetClosePositionWithRadius(investigatePoint, 5f);

            if (investigatePoint == Vector2.zero || !ValidPathTo(investigatePoint))
            {
                Debug.Log(_character.Code + " Cannot investigate attack origin");
                return;
            }
        }
        
        Debug.Log("investigating attack origin direction");
        Debug.DrawLine(transform.position, investigatePoint, Color.green, 5f);
        _behaviorTree.SetData("destinationPoint", investigatePoint);
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
    
    public void SetIdleStatus(bool status)
    {
        _isIdle = status;
    }

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public bool IsIdle => _isIdle;
    public AIWeaponAimManager AIWeaponAimManager => _aiWeaponAimManager;
    public bool HasPath => _navMeshAgent.hasPath;
}
