using UnityEngine;
using BehaviorTree;

public class TaskSleep : Node
{
    AICharacterManager _aiCharacterManager;

    public TaskSleep(AICharacterManager aiCharacterManager)
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        root.Sleep();
        _aiCharacterManager.OnSleep();
        state = NodeState.SUCCESS;
        return state;
    }
}