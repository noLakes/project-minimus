using UnityEngine;

using BehaviorTree;

public class TaskMoveToDestination : Node
{
    CharacterManager manager;

    public TaskMoveToDestination(CharacterManager manager) : base()
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        object destinationPoint = root.GetData("destinationPoint");

        Vector3 destination = (Vector3) destinationPoint;
        // check to see if the destination point was changed
        // and we need to re-update the agent's destination
        if (destination != manager.navMeshAgent.destination && Vector3.Distance(destination, manager.navMeshAgent.destination) > 0.5f)
        {
            bool canMove = manager.TryMove(destination);
            state = canMove ? NodeState.RUNNING : NodeState.FAILURE;
            if(state == NodeState.FAILURE) Debug.Log("Cannot reach: " + destination);
            return state;
        }

        // check to see if the agent has reached the destination
        float d = Vector3.Distance(manager.transform.position, manager.navMeshAgent.destination);
        if (d <= manager.navMeshAgent.stoppingDistance)
        {
            root.ClearData("destinationPoint");
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}