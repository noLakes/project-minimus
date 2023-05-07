using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    UnitManager manager;

    public CheckEnemyInAttackRange(UnitManager manager) : base()
    {
        this.manager = manager;
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

        // (in case the target object is gone - for example it died
        // and we haven't cleared it from the data yet)
        if (!target)
        {
            //Debug.Log("CHECK ENEMY RANGE FAILED. TARGET GONE");
            root.ClearData("currentTarget");
            state = NodeState.FAILURE;
            return state;
        }

        float attackRange = manager.Unit.attackRange;

        bool isInRange = Utility.CalcUnitTargetDistance(manager, target.GetComponent<UnitManager>()) <= attackRange;
        //state = isInRange ? NodeState.SUCCESS : NodeState.FAILURE;

        if(isInRange)
        {
            root.ClearData("followDestination");
            state = NodeState.SUCCESS;
            manager.StopMoving();
            //Debug.Log("Attack target IN RANGE");
        }
        else
        {
            state = NodeState.FAILURE;
            //Debug.Log("Attack target NOT IN RANGE");
        }

        return state;
    }
}