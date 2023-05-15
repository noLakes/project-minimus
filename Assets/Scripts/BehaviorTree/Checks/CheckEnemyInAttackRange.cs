using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    private AICharacterManager _aiCharacterManager;

    public CheckEnemyInAttackRange(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = GetData("currentTarget");
        if (currentTarget == null)
        {
            _state = NodeState.FAILURE;
            return State;
        }

        Transform target = (Transform)currentTarget;

        // (in case the target object is gone - for example it died
        // and we haven't cleared it from the data yet)
        if (!target)
        {
            Debug.Log("CHECK ENEMY RANGE FAILED. TARGET GONE");
            ClearData("currentTarget");
            _state = NodeState.FAILURE;
            return State;
        }

        float attackRange = _aiCharacterManager.CurrentWeapon.Stats.Range;

        bool isInRange = Vector2.Distance(_aiCharacterManager.transform.position, target.position) <= attackRange;

        if(isInRange)
        {
            ClearData("followDestination");
            _state = NodeState.SUCCESS;
            _aiCharacterManager.StopMoving();
            Debug.Log("Attack target IN RANGE");
        }
        else
        {
            _state = NodeState.FAILURE;
            Debug.Log("Attack target NOT IN RANGE");
        }

        return _state;
    }
}