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
        object destinationPoint = root.GetData("destinationPoint");

        Vector2 destination = (Vector2)destinationPoint;
        // check to see if the destination point was changed
        if (destination != (Vector2)_aiCharacterManager.NavMeshAgent.destination)
        {
            var canMove = _aiCharacterManager.TryMove(destination);
            state = canMove ? NodeState.RUNNING : NodeState.FAILURE;
            if(state == NodeState.FAILURE) Debug.Log("Cannot reach: " + destination);
            Debug.Log("EXIT 1: " + state);
            return state;
        }

        // check to see if the agent has reached the destination
        var d = Vector2.Distance(_aiCharacterManager.transform.position, _aiCharacterManager.NavMeshAgent.destination);
        if (d <= _aiCharacterManager.NavMeshAgent.stoppingDistance)
        {
            root.ClearData("destinationPoint");
            _aiCharacterManager.StopMoving();
            state = NodeState.SUCCESS;
            Debug.Log("EXIT 2: " + state);
            return state;
        }
        
        state = NodeState.RUNNING;
        Debug.Log("EXIT 3: " + state);
        return state;
    }
}