using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskFollow : Node
{
    private AIController _aiController;
    private Vector2 _lastTargetPosition;

    public TaskFollow(AIController aiController) : base()
    {
        _aiController = aiController;
        _lastTargetPosition = Vector2.zero;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("Following");
        Transform currentTarget = (Transform)root.GetData("currentTarget");

        Vector2 targetPosition = currentTarget.position;

        if (!_aiController.ValidPathTo(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector2.zero)
            {
                _lastTargetPosition = Vector3.zero;
                state = NodeState.FAILURE;
                return state;
            }
        }
        
        _aiController.TryMove(targetPosition);
        _lastTargetPosition = targetPosition;

        // check if the agent has reached destination
        float d = Vector3.Distance(_aiController.transform.position, targetPosition);
        if (d <= _aiController.NavMeshAgent.stoppingDistance)
        {
            _lastTargetPosition = Vector3.zero;
            //Debug.Log("SUCCESS FOLLOW: REACHED");
            //root.ClearData("currentTarget");
            _aiController.StopMoving();
            state = NodeState.SUCCESS;
            return state;
        }

        //Debug.Log("RUNNING FOLLOW");
        state = NodeState.RUNNING;
        return state;
    }
}