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
        state = _aiCharacterManager.IsIdle ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}