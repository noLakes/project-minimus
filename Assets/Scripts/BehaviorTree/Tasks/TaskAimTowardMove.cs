using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAimTowardMove : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskAimTowardMove(AICharacterManager aiCharacterManager)
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        object destinationPoint = GetData("destinationPoint");

        if (destinationPoint == null)
        {
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }
        
        Vector2 destination = (Vector2)destinationPoint;
        
        _aiCharacterManager.AICharacterWeaponAimer.AimTowards(destination);
        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}
