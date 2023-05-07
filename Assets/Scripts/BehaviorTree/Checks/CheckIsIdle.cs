using UnityEngine;

using BehaviorTree;

public class CheckIsIdle: Node
{
    private CharacterManager manager;

    public CheckIsIdle(CharacterManager manager)
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        state = manager.isIdle ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}