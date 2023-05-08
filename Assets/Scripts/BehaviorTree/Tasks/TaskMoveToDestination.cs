using UnityEngine;

using BehaviorTree;

public class TaskMoveToDestination : Node
{
    private AIController _aiController;

    public TaskMoveToDestination(AIController aiController) : base()
    {
        _aiController = aiController;
    }

    public override NodeState Evaluate()
    {
        object destinationPoint = root.GetData("destinationPoint");

        Vector2 destination = (Vector2)destinationPoint;
        // check to see if the destination point was changed
        // and we need to re-update the agent's destination
        if (destination != (Vector2)_aiController.NavMeshAgent.destination && Vector2.Distance(destination, _aiController.NavMeshAgent.destination) > 0.5f)
        {
            var canMove = _aiController.TryMove(destination);
            state = canMove ? NodeState.RUNNING : NodeState.FAILURE;
            if(state == NodeState.FAILURE) Debug.Log("Cannot reach: " + destination);
            return state;
        }

        // check to see if the agent has reached the destination
        var d = Vector2.Distance(_aiController.transform.position, _aiController.NavMeshAgent.destination);
        if (d <= _aiController.NavMeshAgent.stoppingDistance)
        {
            root.ClearData("destinationPoint");
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}