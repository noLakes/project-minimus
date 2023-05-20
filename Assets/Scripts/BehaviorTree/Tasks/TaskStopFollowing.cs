using UnityEngine;

using BehaviorTree;

public class TaskStopFollowing: Node
{
    public override NodeState Evaluate()
    {
        ClearData("followDestination");
        _state = NodeState.SUCCESS;
        return _state;
    }
}