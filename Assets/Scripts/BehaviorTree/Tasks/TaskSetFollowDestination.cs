using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskSetFollowDestination : Node
{
    CharacterManager manager;

    public TaskSetFollowDestination(CharacterManager manager) : base()
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        Transform currentTarget = (Transform)root.GetData("currentTarget");

        Vector3 targetPosition = Utility.TargetClosestPosition(manager, currentTarget.GetComponent<UnitManager>());

        if (!manager.ValidPathTo(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector3.zero)
            {
                root.ClearData("followDestination");
                root.ClearData("currentTarget");
                manager.StopMoving();
                state = NodeState.FAILURE;
                return state;
            }
        }
        
        root.SetData("followDestination", targetPosition);
        manager.TryMove(targetPosition);

        state = NodeState.RUNNING;
        return state;
    }
}