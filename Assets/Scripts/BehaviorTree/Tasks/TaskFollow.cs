using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskFollow : Node
{
    private AICharacterManager _aiCharacterManager;
    private Vector2 _lastTargetPosition;

    public TaskFollow(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
        _lastTargetPosition = Vector2.zero;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("Following");
        Transform currentTarget = (Transform)GetData("currentTarget");

        Vector2 targetPosition = currentTarget.position;

        if (!_aiCharacterManager.ValidPathTo(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector2.zero)
            {
                _lastTargetPosition = Vector3.zero;
                _state = NodeState.FAILURE;
                return _state;
            }
        }
        
        _aiCharacterManager.TryMove(targetPosition);
        _lastTargetPosition = targetPosition;

        // check if the agent has reached destination
        float d = Vector3.Distance(_aiCharacterManager.transform.position, targetPosition);
        if (d <= _aiCharacterManager.NavMeshAgent.stoppingDistance)
        {
            _lastTargetPosition = Vector3.zero;
            //Debug.Log("SUCCESS FOLLOW: REACHED");
            //root.ClearData("currentTarget");
            _aiCharacterManager.StopMoving();
            _state = NodeState.SUCCESS;
            return _state;
        }

        //Debug.Log("RUNNING FOLLOW");
        _state = NodeState.RUNNING;
        return _state;
    }
}