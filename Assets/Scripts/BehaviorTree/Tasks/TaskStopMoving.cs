using UnityEngine;

using BehaviorTree;

public class TaskStopMoving : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskStopMoving(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        _aiCharacterManager.StopMoving();

        state = NodeState.SUCCESS;
        return state;
    }
}