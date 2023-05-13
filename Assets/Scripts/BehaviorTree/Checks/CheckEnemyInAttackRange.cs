using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    AICharacterManager _aiCharacterManager;

    public CheckEnemyInAttackRange(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
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

        float attackRange = _aiCharacterManager.CurrentWeapon.Stats.Range;

        bool isInRange = Vector2.Distance(_aiCharacterManager.transform.position, target.position) <= attackRange;

        if(isInRange)
        {
            root.ClearData("followDestination");
            state = NodeState.SUCCESS;
            _aiCharacterManager.StopMoving();
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