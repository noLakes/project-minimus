using UnityEngine;

using BehaviorTree;

public class CheckHasCastingAbility: Node
{
    public override NodeState Evaluate()
    {
        object castingAbility = root.GetData("castingAbility");
        if (castingAbility == null)
        {
            root.ClearData("castingAbility");
            root.ClearData("castingTargetData");
            state = NodeState.FAILURE;
            return state;
        }
        if (!((AbilityManager) castingAbility))
        {
            root.ClearData("castingAbility");
            root.ClearData("castingTargetData");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}