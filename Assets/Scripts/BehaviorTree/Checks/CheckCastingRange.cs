using UnityEngine;

using BehaviorTree;

public class CheckCastingRange : Node
{
    UnitManager manager;

    public CheckCastingRange(UnitManager manager)
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        object castingAbility = root.GetData("castingAbility");
        if (castingAbility == null)
        {
            //Debug.Log("No casting ability found. FAIL in range check");
            state = NodeState.FAILURE;
            return state;
        }

        AbilityManager abilityManager = (AbilityManager)castingAbility;
        if (!abilityManager)
        {
            //Debug.Log("No casting ability found. FAIL in range check");
            root.ClearData("castingAbility");
            state = NodeState.FAILURE;
            return state;
        }

        if (!abilityManager.targetRequired)
        {
            //Debug.Log("No target needed. Pass range check");
            state = NodeState.SUCCESS;
            return state;
        }

        object castingTargetData = root.GetData("castingTargetData");
        if (castingTargetData == null)
        {
            //Debug.Log("No target data set. Failed range check");
            state = NodeState.FAILURE;
            return state;
        }

        TargetData tData = (TargetData)root.GetData("castingTargetData");
        if (abilityManager.hasRange && !abilityManager.InCastingRange(tData))
        {
            //Debug.Log("Target not in range. FAIL range check.");
            state = NodeState.FAILURE;
            return state;
        }

        manager.StopMoving();

        //Debug.Log("Target is in legal range");
        state = NodeState.SUCCESS;
        return state;
    }
}