using BehaviorTree;
using UnityEngine;

public class CheckHasDestination : Node
{
    public override NodeState Evaluate()
    {
        object destinationPoint = GetData("destinationPoint");
        if (destinationPoint == null)
        {
            //Debug.Log("No destination");
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return State;
        }
        
        //Debug.Log("Has Destination");
        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}