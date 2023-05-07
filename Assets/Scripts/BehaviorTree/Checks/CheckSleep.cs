using UnityEngine;

using BehaviorTree;

public class CheckSleep: Node
{
    public override NodeState Evaluate()
    {
        state = root.awake ? NodeState.FAILURE : NodeState.SUCCESS;
        return state;
    }
}