using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAimAtTarget : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskAimAtTarget(AICharacterManager aiCharacterManager)
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = GetData("currentTarget");
        var targetTransform = (Transform)currentTarget;
        
        _aiCharacterManager.AICharacterWeaponAimer.AimTowards(targetTransform.position);
        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}
