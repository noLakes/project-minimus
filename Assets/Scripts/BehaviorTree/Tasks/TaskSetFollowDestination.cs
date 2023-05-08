using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskSetFollowDestination : Node
{
    private AIController _aiController;

    public TaskSetFollowDestination(AIController aiController) : base()
    {
        _aiController = aiController;
    }

    public override NodeState Evaluate()
    {
        Transform currentTarget = (Transform)root.GetData("currentTarget");

        Vector2 targetPosition = currentTarget.position;

        if (!_aiController.ValidPathTo(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector2.zero)
            {
                root.ClearData("followDestination");
                root.ClearData("currentTarget");
                _aiController.StopMoving();
                state = NodeState.FAILURE;
                return state;
            }
        }
        
        root.SetData("followDestination", targetPosition);
        _aiController.TryMove(targetPosition);

        state = NodeState.RUNNING;
        return state;
    }
}