using BehaviorTree;
using UnityEngine;

public class CheckHasAttackMove : Node
{
    public override NodeState Evaluate()
    {
        object attackMove = root.GetData("attackMove");

        if (attackMove == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}