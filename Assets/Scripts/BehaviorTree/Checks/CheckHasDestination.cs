using BehaviorTree;
using UnityEngine;

public class CheckHasDestination : Node
{
    public override NodeState Evaluate()
    {
        object destinationPoint = root.GetData("destinationPoint");
        if (destinationPoint == null)
        {
            //Debug.Log("No destination");
            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}