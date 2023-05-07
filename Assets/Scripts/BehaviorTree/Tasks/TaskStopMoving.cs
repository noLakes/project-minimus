using UnityEngine;

using BehaviorTree;

public class TaskStopMoving : Node
{
    private AIController _aiController;

    public TaskStopMoving(AIController aiController) : base()
    {
        _aiController = aiController;
    }

    public override NodeState Evaluate()
    {
        _aiController.StopMoving();

        state = NodeState.SUCCESS;
        return state;
    }
}