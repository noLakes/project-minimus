using UnityEngine;

using BehaviorTree;

public class TaskSetIdle : Node
{
    private AIController _aiController;

    public TaskSetIdle(AIController aiController) : base()
    {
        _aiController = aiController;
    }

    public override NodeState Evaluate()
    {
        if (!_aiController.IsIdle)
        {
            //_aiController.ActAsNavObstacle(); may not be needed in this project
            _aiController.SetIdleStatus(true);
            Debug.Log("Set Idle");
        }

        state = NodeState.SUCCESS;
        return state;
    }
}