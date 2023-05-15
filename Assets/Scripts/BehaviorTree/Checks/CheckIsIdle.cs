using UnityEngine;

using BehaviorTree;

public class CheckIsIdle: Node
{
    private AICharacterManager _aiCharacterManager;

    public CheckIsIdle(AICharacterManager aiCharacterManager)
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        _state = _aiCharacterManager.IsIdle ? NodeState.SUCCESS : NodeState.FAILURE;
        return _state;
    }
}