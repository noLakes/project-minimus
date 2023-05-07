using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskFollow : Node
{
    CharacterManager manager;
    Vector3 lastTargetPosition;

    public TaskFollow(CharacterManager manager) : base()
    {
        this.manager = manager;
        lastTargetPosition = Vector3.zero;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("Following");
        Transform currentTarget = (Transform)root.GetData("currentTarget");

        Vector2 targetPosition = Utility.TargetClosestPosition(manager, currentTarget.GetComponent<UnitManager>());

        if (!manager.ValidPathTo(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector3.zero)
            {
                lastTargetPosition = Vector3.zero;
                state = NodeState.FAILURE;
                return state;
            }
        }
        
        manager.TryMove(targetPosition);
        lastTargetPosition = targetPosition;

        // check if the agent has reached destination
        float d = Vector3.Distance(manager.transform.position, targetPosition);
        if (d <= manager.navMeshAgent.stoppingDistance)
        {
            lastTargetPosition = Vector3.zero;
            //Debug.Log("SUCCESS FOLLOW: REACHED");
            //root.ClearData("currentTarget");
            manager.StopMoving();
            state = NodeState.SUCCESS;
            return state;
        }

        //Debug.Log("RUNNING FOLLOW");
        state = NodeState.RUNNING;
        return state;
    }
}