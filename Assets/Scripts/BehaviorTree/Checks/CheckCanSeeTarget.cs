using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorTree;

public class CheckCanSeeTarget : Node
{
    // this class may need to be changed to handle scanning for multiple enemies in future
    
    private AICharacterManager _aiCharacterManager;
    private float _fovRadius;
    private Transform _transform;
    private int _layerMask = ~(1 << 7 | 1 << 9);
    
    public CheckCanSeeTarget(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
        _fovRadius = _aiCharacterManager.Character.Stats.fovRadius;
        _transform = _aiCharacterManager.transform;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = GetData("currentTarget");
        Transform target = (Transform)currentTarget;
        
        if (!target)
        {
            ClearData("currentTarget");
            ClearData("targetLastSeenPos");
            _state = NodeState.FAILURE;
            return _state;
        }

        Vector2 targetPos = target.position;
            
        var targetDistance = Vector2.Distance(
            _transform.position,
            targetPos
            );

        if (targetDistance <= _fovRadius && Utility.HasLineOfSight(_transform, target, _fovRadius, _layerMask))
        {
            Parent.Parent.SetData("targetLastSeenPos", targetPos);
            _state = NodeState.SUCCESS;
            return _state;
        }
        
        Debug.Log("Cannot see Target");
        ClearData("currentTarget");
        _state = NodeState.FAILURE;
        return _state;
    }
}