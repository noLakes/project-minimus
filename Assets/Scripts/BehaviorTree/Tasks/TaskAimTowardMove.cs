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
        Vector2 destination = (Vector2)destinationPoint;
        
        _aiCharacterManager.AIWeaponAimManager.AimTowards(destination);
        _state = NodeState.SUCCESS;
        return _state;
    }
}
