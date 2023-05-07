using UnityEngine;

using BehaviorTree;

public class CheckTargetVisible : Node
{
    UnitManager manager;
    bool playerOwned;
    float fovRadius;

    public CheckTargetVisible(UnitManager manager)
    {
        this.manager = manager;
        fovRadius = manager.Unit.data.fieldOfView;
        playerOwned = manager.owner == Game.Instance.humanPlayerID;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = root.GetData("currentTarget");
        if (currentTarget == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)currentTarget;

        if (playerOwned && target.TryGetComponent<FogRendererToggler>(out FogRendererToggler ftg) && ftg.IsVisible())
        {
            state = NodeState.SUCCESS;
            return state;
        }
        else if (manager.CanSee(target))
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}