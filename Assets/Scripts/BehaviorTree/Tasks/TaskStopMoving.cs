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

        _state = NodeState.SUCCESS;
        return _state;
    }
}