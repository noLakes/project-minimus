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
        Transform currentTarget = (Transform)root.GetData("currentTarget");

        Vector2 targetPosition = currentTarget.position;

        if (!_aiCharacterManager.ValidPathTo(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector2.zero)
            {
                root.ClearData("followDestination");
                root.ClearData("currentTarget");
                _aiCharacterManager.StopMoving();
                state = NodeState.FAILURE;
                return state;
            }
        }
        
        root.SetData("followDestination", targetPosition);
        _aiCharacterManager.TryMove(targetPosition);

        state = NodeState.RUNNING;
        return state;
    }
}