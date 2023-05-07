using UnityEngine;

using BehaviorTree;

public class CheckAwake: Node
{
    public override NodeState Evaluate()
    {
        state = root.awake ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}