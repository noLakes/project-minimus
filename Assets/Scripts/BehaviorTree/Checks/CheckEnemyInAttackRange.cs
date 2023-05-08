using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    AIController _aiController;

    public CheckEnemyInAttackRange(AIController aiController) : base()
    {
        _aiController = aiController;
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
            Debug.Log("CHECK ENEMY RANGE FAILED. TARGET GONE");
            root.ClearData("currentTarget");
            state = NodeState.FAILURE;
            return state;
        }

        float attackRange = _aiController.CharacterManager.CurrentWeapon.Stats.Range;

        bool isInRange = Vector2.Distance(_aiController.transform.position, target.position) <= attackRange;

        if(isInRange)
        {
            root.ClearData("followDestination");
            state = NodeState.SUCCESS;
            _aiController.StopMoving();
            Debug.Log("Attack target IN RANGE");
        }
        else
        {
            state = NodeState.FAILURE;
            Debug.Log("Attack target NOT IN RANGE");
        }

        return state;
    }
}