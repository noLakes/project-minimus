using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskSetFollowDestination : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskSetFollowDestination(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        Transform currentTarget = (Transform)GetData("currentTarget");

        Vector2 targetPosition = currentTarget.position;

        if (!_aiCharacterManager.ValidPathTo(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector2.zero)
            {
                ClearData("followDestination");
                ClearData("currentTarget");
                _aiCharacterManager.StopMoving();
                _state = NodeState.FAILURE;
                ThrowResultToDebugCallStack(GetType().Name, _state);
                return _state;
            }
        }
        
        Parent.Parent.SetData("followDestination", targetPosition);
        _aiCharacterManager.TryMove(targetPosition);

        _state = NodeState.RUNNING;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}