using UnityEngine;

using BehaviorTree;

public class TaskMoveToDestination : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskMoveToDestination(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        object destinationPoint = GetData("destinationPoint");

        Vector2 destination = (Vector2)destinationPoint;
        // check to see if the destination point was changed
        if (destination != (Vector2)_aiCharacterManager.NavMeshAgent.destination)
        {
            var canMove = _aiCharacterManager.TryMove(destination);
            _state = canMove ? NodeState.RUNNING : NodeState.FAILURE;
            if(_state == NodeState.FAILURE) Debug.Log("Cannot reach: " + destination);
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }

        // check to see if the agent has reached the destination
        var d = Vector2.Distance(_aiCharacterManager.transform.position, _aiCharacterManager.NavMeshAgent.destination);
        if (d <= _aiCharacterManager.NavMeshAgent.stoppingDistance)
        {
            ClearData("destinationPoint");
            _aiCharacterManager.StopMoving();
            _state = NodeState.SUCCESS;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }
        
        _state = NodeState.RUNNING;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}