using UnityEngine;

using BehaviorTree;

public class CheckIsIdle: Node
{
    private AIController _aiController;

    public CheckIsIdle(AIController aiController)
    {
        _aiController = aiController;
    }

    public override NodeState Evaluate()
    {
        state = _aiController.IsIdle ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}