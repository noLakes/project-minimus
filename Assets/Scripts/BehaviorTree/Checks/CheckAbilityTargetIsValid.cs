using UnityEngine;

using BehaviorTree;

public class CheckAbilityTargetIsValid : Node
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

        AbilityManager abilityManager = (AbilityManager)castingAbility;

        if (!abilityManager)
        {
            //Debug.Log("FAILED IN CHECKABILITYTARGETVALID");
            root.ClearData("castingAbility");
            root.ClearData("castingTargetData");
            state = NodeState.FAILURE;
            return state;
        }

        if (!abilityManager.targetRequired)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        object castingTargetData = root.GetData("castingTargetData");
        if (castingTargetData == null)
        {
            //Debug.Log("FAILED IN CHECKABILITYTARGETVALID");
            root.ClearData("castingAbility");
            root.ClearData("castingTargetData");
            state = NodeState.FAILURE;
            return state;
        }

        TargetData tData = (TargetData)root.GetData("castingTargetData");
        if(!abilityManager.ValidTargetData(tData))
        {
            Debug.Log("FAILED IN CHECKABILITYTARGETVALID");
            root.ClearData("castingAbility");
            root.ClearData("castingTargetData");
            state = NodeState.FAILURE;
            return state;
        }

        //Debug.Log("Target valid for " + abilityManager.abilityName);
        state = NodeState.SUCCESS;
        return state;
    }
}