using UnityEngine;
using BehaviorTree;

public class TaskSleep : Node
{
    AIController _aiController;

    public TaskSleep(AIController aiController)
    {
        _aiController = aiController;
    }

    public override NodeState Evaluate()
    {
        root.Sleep();
        _aiController.OnSleep();
        state = NodeState.SUCCESS;
        return state;
    }
}